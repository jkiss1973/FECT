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

namespace FECT.Tokenizer.Detectors
{

	public class TwoCharMultiLine_Detector: BaseDetector
	{
		private bool BeginningDetected = false;
		private bool EndDetected = false;
		private string begchars = "";
		private string endchars = "";
		
		
		public string BeginningChars {
			
			get
			{
				return(begchars);
			}
			
			set
			{
				if (value.Length == 2)
						begchars = value;
				else {
					throw new TokenizerException(FECT.ExceptionStength.ERROR,"Invalid number of chars [ must  be 2 ]");
				}
			}
		}
		public string EndingChars {
			get { return endchars; }
		
			set
			{
				if (value.Length == 2)
						endchars = value;
				else {
					throw new TokenizerException(FECT.ExceptionStength.ERROR,"Invalid number of chars [ must  be 2 ]");
				}
			}
		
		}
		
		
		public override TokenState Process(char current, char peek)
		{
			TokenState retval = TokenState.REJECTED;
			
			
			if (BeginningDetected == false)
			{
				if (current == begchars[0] && peek == begchars[1])
				{
					BeginningDetected = true;
					retval = TokenState.MORE;
				}
			}
			else
			{
				
				retval = TokenState.MORE;
				
				if (EndDetected == false)
				{
					if (current == endchars[0] && peek == endchars[1])
					{
						EndDetected = true;
					}
					
				}
				else 
				{
					if (current == endchars[1])
						retval = CompletedState;
					else
					{
						retval = TokenState.REJECTED;
					}
					
				}
				
			}
			
			return(retval);
		}
		
		
		public override void Reset ()
		{
			base.Reset ();
			
			BeginningDetected = false;
			EndDetected = false;
		}
		
	}
	

	public class SingleCharMultiLine_Detector: BaseDetector
	{
		private bool BeginningDetected = false;
		private string begchars = "";
		private string endchars = "";
		
		
		public string BeginningChars {
			
			get
			{
				return(begchars);
			}
			
			set
			{
				if (value.Length == 1)
						begchars = value;
				else {
					throw new TokenizerException(FECT.ExceptionStength.ERROR,"Invalid number of chars [ must  be 1 ]");
				}
			}
		}
		public string EndingChars {
			get { return endchars; }
		
			set
			{
				if (value.Length == 1)
						endchars = value;
				else {
					throw new TokenizerException(FECT.ExceptionStength.ERROR,"Invalid number of chars [ must  be 1 ]");
				}
			}
		
		}
		
		
		public override TokenState Process(char current, char peek)
		{
			TokenState retval = TokenState.REJECTED;
			
			
			if (BeginningDetected == false)
			{
				if (current == begchars[0])
				{
					BeginningDetected = true;
					retval = TokenState.MORE;
				}
			}
			else
			{
				
				retval = TokenState.MORE;
				
				if (current == endchars[0] )
				{
					retval = CompletedState;
				}
					
			}
			
			return(retval);
		}
		
		
		public override void Reset ()
		{
			base.Reset ();
			
			BeginningDetected = false;

		}
		
	}
	
	
}

