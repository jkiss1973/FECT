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
	public class Identifier_Detector: SingleStream_Detector
	{
		
		public bool AllowNumberic	{get; set;}  // Allow Numberics after the the first char is an Alpha
		private bool gotFirstChar = false;
		
		public Identifier_Detector()
		{
			AllowNumberic = false;
			gotFirstChar = false;
		}
		
		public override TokenState Process(char current, char peek)
		{
			TokenState retval = TokenState.REJECTED;
						
			if (MoreChars.IndexOf(current) > -1)
			{
				retval = TokenState.MORE;
				
				gotFirstChar = true;
				
				if (CompleteChars.IndexOf(peek) > -1)
				{
					retval = CompletedState;
				}
				
				
			}
			else if (AllowNumberic == true && gotFirstChar == true && (current >= '0' && current <= '9'))
			{
				retval = TokenState.MORE;
				
				if (CompleteChars.IndexOf(peek) > -1)
				{
					retval = CompletedState;
				}
			}
			
			return(retval);
		}
		
		public override void Reset ()
		{
			base.Reset ();
			
			gotFirstChar = false;
			
		}
		
	}
	
	
}

