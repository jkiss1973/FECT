/* *******************************************************************************
    FECT
    Copyright (C) 2014 John Kiss

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301
    USA
    
********************************************************************************* */ 
using System;
using System.Collections.Generic;
using FECT.Tokenizer.Detectors;

namespace FECT.Tokenizer
{
	
	public class LineTokenizer
	{
			
		protected List<DetectorInterface> 	proDetectors;		// List of all Detectors/Filters
		protected List<DetectorInterface> 	proMoreList;		// List of Detectors in the "MORE" state
		protected List<DetectorInterface> 	proAcceptedList;	// List of Detectors in the "ACCEPTED" state
		protected List<DetectorInterface> 	proIgnoreList;		// List of Detectors in the "IGNORE" state
		protected List<DetectorInterface> 	proFilteredList;	// List of Detectors in the "FILTER" state
		
		public List<Token> 					TokenList;			// List of all Accepted Tokens

		protected string 		   			proTokenString;

		
		/* ******************* PROPERTIES ******************* */
		public bool							KeepAccepted {get; set; }
		public bool							KeepFiltered {get; set; }
		public bool							KeepIgnored {get; set;}
		public bool							KeepErrors {get; set;}
		public TrimStyle 					EnableTrim {get; set;}
		
				
		public event FECTErrorHandler			ErrorEvent;
		
		/* Class Properties */
		
		public LineTokenizer()
		{
			proDetectors = new List<DetectorInterface>();
			proMoreList = new List<DetectorInterface>();
			proAcceptedList = new List<DetectorInterface>();
			proIgnoreList = new List<DetectorInterface>();
			proFilteredList = new List<DetectorInterface>();
			
			TokenList = new List<Token>();
			
			KeepAccepted = true;
			KeepFiltered = false;
			KeepIgnored = false;
			KeepErrors = false;
			
			EnableTrim = TrimStyle.NONE;
		}
			
		
		protected void ResetDetectors()
		{
			try
			{
				proMoreList.Clear();
				proAcceptedList.Clear();
				proIgnoreList.Clear();
				proFilteredList.Clear();
				proTokenString = "";
				
				foreach (DetectorInterface detector in proDetectors)
				{
					detector.Reset();
					
					proMoreList.Add(detector);
				}
			}
			catch
			{
				throw new TokenizerException(FECT.ExceptionStength.ERROR,"Error reseting detectors");
			}
		}
		
		protected void DeactivateDetector(DetectorInterface active)
		{
			active.SetActiveState(DetectorState.INACTIVE);
		}
		
		virtual public void Begin()
		{
			try
			{
				ResetDetectors();
				
			}
			catch (FECTException e)
			{
				throw e;
			}
			catch
			{
				throw new TokenizerException(FECT.ExceptionStength.FATAL,"Unknown error in tokenizer begin"); 
			}
		}

		virtual public void End()
		{

			try
			{
				ResetDetectors();
			}
			catch (FECTException e)
			{
				throw e;
			}
			catch
			{
				throw new TokenizerException(FECT.ExceptionStength.FATAL,"Unknown error in tokenizer end"); 
			}
		
		}
		
		protected void RemoveInactive()
		{
			try
			{
				foreach (DetectorInterface detector in proDetectors)
				{
					if (detector.GetActiveState() == DetectorState.INACTIVE)
					{
						proMoreList.Remove(detector);
					}
				}
			}
			catch
			{
				throw new TokenizerException(FECT.ExceptionStength.FATAL,"error setting detector to inactive"); 
			}
		}
	
		protected void Trim(ref string line)
		{
			try{
				string newline = line;
				Int32 new_index = 0;
				Int32 len = newline.Length -1;
				
				line = "";
			
			//FIXME, a regular expression can do this work.
			
				if (len > 0)
				{
					while (new_index <= len)
					{
						char ch = newline[new_index];
						
						if (ch != (char)TokenAsciiCode.SPC && ch != (char)TokenAsciiCode.HT)
						{
							line += newline[new_index];
						}
						
						new_index++;
					}
				}
			}
			catch
			{
				throw new TokenizerException(FECT.ExceptionStength.ERROR,"Trim error"); 
			}
			
		}
		
		protected bool DetectorsFinished()
		{
			bool retval = false;
			
			if (proMoreList.Count == 0)
			{
				retval = true;
			}
				
			return(retval);
		}
			
		
		virtual public void Process(string line, UInt32 linenumber)
		{
			Int32 index = 0; 			// Character index in string

			try
			{
				if (EnableTrim != TrimStyle.NONE)
					Trim(ref line);
	
				Int32 len = line.Length -1; // For performance, pre calculate string length;
				UInt32 pos = 1;
				
				// Start processing the stream of characters in the line
				foreach (char ch in line)
				{
					
					char peek = (char)TokenAsciiCode.EOL; //Init as End of line
					
					//If not at the end of line yet set peek char to the next character
					if (index < len)
						peek = line[index+1];
					
					proTokenString += ch;
									
					
					foreach(DetectorInterface detector in proMoreList)
					{
					
						TokenState state = detector.Process(ch,peek);
						
						switch(state)
						{
							case TokenState.ACCEPTED:
								proAcceptedList.Add(detector);
								DeactivateDetector(detector);
								break;
							case TokenState.IGNORE:
								proIgnoreList.Add(detector);
								DeactivateDetector(detector);
								break;
							case TokenState.FILTERED:
							    proFilteredList.Add(detector);
								DeactivateDetector(detector);
								break;
							case TokenState.REJECTED:
								DeactivateDetector(detector);
								break;
							case TokenState.MORE:
								break;
						}
						
					} 
				
					// Remove inactive detectors from the MORE list.
					RemoveInactive();
	
					if (proAcceptedList.Count == 0 && proMoreList.Count == 0 && proIgnoreList.Count == 0 && proFilteredList.Count == 0)
					{
						// Nobody loves this token, so sad :(
						
						Token t = new Token {Value = proTokenString, State=TokenState.UNKNOWN, Position=new TokenPosition{LineNumber = linenumber,CharStartPosition = pos}, Type=FECT.TokenTypes.DefaultTokenType };

						if (KeepErrors == true)
						{
							
							TokenList.Add(t);
							Console.WriteLine(t.ToString());
						}
						
						ErrorEvent(Phase.TOKENIZER,(Int32)FECT.Tokenizer.ErrorCode.UNKNOWN_TOKEN,t,"Unknown token");
						
						ResetDetectors();
						pos += (UInt32)proTokenString.Length;
						proTokenString = "";
						
	
					}
					else if (proAcceptedList.Count > 0 || proIgnoreList.Count > 0 || proFilteredList.Count > 0)
					{
						// There is one ore more detectors that accepted the token and others may still be interested */
						// Using a detector weight property, find out the top weight, the highest weight wins.
						// If there is a tie, the first found wins.
						
						UInt32 top_weight =  0;
						DetectorInterface top_detector;
						bool more_is_top = false;
						bool ignore_is_top = false;
						bool filtered_is_top = false;
						
						// Search the accepted list and find the highest weight.
						// If there is a tie, the first in the list wins.					
						foreach(DetectorInterface detector in proAcceptedList)
						{
							UInt32 weight = detector.GetWeight();
							
							if (weight > top_weight)
							{
								top_weight = weight;
								top_detector = detector;
							}
						}
	
						// Search the Ignore list and find the highest weight.
						// If there is a tie, the first in the list wins.					
						foreach(DetectorInterface detector in proIgnoreList)
						{
							UInt32 weight = detector.GetWeight();
							
							if (weight > top_weight)
							{
								top_weight = weight;
								top_detector = detector;
								ignore_is_top = true;
							}
							else // Doing us all a favor, if this detector has no chance of ever winning then deactivate
							{
								DeactivateDetector(detector);
							}
							
						}

						RemoveInactive();
						
						// Search the filter list and find the highest weight.
						// If there is a tie, the one before wins.					
						foreach(DetectorInterface detector in proFilteredList)
						{
							UInt32 weight = detector.GetWeight();
							
							if (weight > top_weight)
							{
								top_weight = weight;
								top_detector = detector;
								filtered_is_top = true;
							}
							else // Doing us all a favor, if this detector has no chance of ever winning then deactivate
							{
								DeactivateDetector(detector);
							}
							
						}
						
						RemoveInactive();
						
						// If there are others still interested in the stream check their weights.
						// If there is a tie, the first found in the list wins.
						foreach(DetectorInterface detector in proMoreList)
						{
							UInt32 weight = detector.GetWeight();
							
							if (weight > top_weight)
							{
								top_weight = weight;
								top_detector = detector;
								more_is_top = true;
							}
							else // Doing us all a favor, if this detector has no chance of ever winning then deactivate
							{
								DeactivateDetector(detector);
							}
						}
						
						RemoveInactive();
						
						
						
						// This means an accepted detector has won.
						if (more_is_top == false && ignore_is_top == false && filtered_is_top == false)
						{
							if (KeepAccepted == true)
							{
								Token t = top_detector.CreateToken(proTokenString, new TokenPosition {LineNumber=linenumber,CharStartPosition=pos} );
								
								TokenList.Add(t);
								
								Console.WriteLine(t.ToString());
							}
							
							pos += (UInt32)proTokenString.Length;

							proTokenString = "";
							ResetDetectors();
							
						}
						else if (ignore_is_top == true)
						{
							
							if (KeepIgnored == true)
							{
								Token t = top_detector.CreateToken(proTokenString, new TokenPosition {LineNumber=linenumber,CharStartPosition=pos});
								
								TokenList.Add(t);

								Console.WriteLine(t.ToString());
							}

							pos += (UInt32)proTokenString.Length;
							proTokenString = "";
							ResetDetectors();
						}
						else if (filtered_is_top == true)
						{
							
							if (KeepFiltered == true)
							{
								Token t = top_detector.CreateToken(proTokenString, new TokenPosition {LineNumber=linenumber,CharStartPosition=pos});
								
								TokenList.Add(t);

								Console.WriteLine(t.ToString());
							}

							pos += (UInt32)proTokenString.Length;
							proTokenString = "";
							ResetDetectors();
						}
						// else do nothing, detector(s) in the more list has a higher weight and are still intersted
						else
						{
							proAcceptedList.Clear();
							proIgnoreList.Clear();
							proFilteredList.Clear();
							
							// In case some in the more list have been deactivated, remove them from the more list.
							RemoveInactive();
						}
						
					}
					
					index++;	
				}
			}
			catch (FECTException e)
			{
				throw e;
			}
			catch
			{
				throw new TokenizerException(FECT.ExceptionStength.FATAL,"Unknown tokenizer error"); 
			}
			
		}
		
		virtual public void AddDetector(DetectorInterface detector)
		{
			try
			{
				proDetectors.Add(detector);
			}
			catch
			{
				throw new TokenizerException(FECT.ExceptionStength.FATAL,"error adding detector to tokenizer class");
			}
				
		}
				
	}
	
	
	
	
	
	
}