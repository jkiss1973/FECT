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
using FECT;


namespace FECT.Tokenizer.Detectors
{
	public abstract class BaseDetector : DetectorInterface
	{
		private	UInt32	priWeight = Defaults.DetectorWeight;
		
		public DetectorState State { get; set; }
		
		public UInt32 TokenID {get; set; }
		
		public UInt32 Weight { 
			get	{ return priWeight;	}
			
			set
			{
				if (value == 0)
				{
					throw new TokenizerException(FECT.ExceptionStength.ERROR,"Set invalid range for property Weight [ must  be 1 or greater]");
				}
				else
					priWeight = value;
			}
		 } 
		
		public TokenState CompletedState { get; set; } 

		
		public BaseDetector()
		{
			Weight = Defaults.DetectorWeight;
			
			TokenID = TokenTypes.DefaultTokenType.ID;
			
			CompletedState = TokenState.ACCEPTED;
		}

		public virtual TokenState Process(char current, char peek)
		{
			TokenState retval = TokenState.REJECTED;
		
			return(retval);
		}
		
		
		public virtual 	void Reset()
		{
			State = DetectorState.ACTIVE;
		}
		
		public virtual DetectorState GetActiveState()
		{
			return(State);
		}
		
		public virtual void	SetActiveState(DetectorState state)
		{
			State = state;
		}
				

		public virtual Token CreateToken(string tokenstr,TokenPosition pos)
		{
			Token retval = new Token {Value=tokenstr,State=CompletedState,Position=pos, Type=FECT.TokenTypes.Find(TokenID)};
						
			return(retval);
		}
		
		
		public virtual UInt32 GetWeight()
		{
			return(Weight);
		}
		
	}
}

