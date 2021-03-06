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
	
	public class Filter_Detector: BaseDetector
	{
		public Filter_Detector()
		{
			CompletedState = TokenState.FILTERED;
		}
		
	}
	
	public class UnSeenCharacterFilter_Detector: Filter_Detector
	{
		
		public override TokenState Process(char current, char peek)
		{
			TokenState retval = TokenState.REJECTED;
			
			if ((Int32)current >= 0 && (Int32)current <= (Int32)TokenAsciiCode.SPC)
			{
				retval = CompletedState;
			}
			
			return(retval);
		}
	}
	

	public class TabFilter_Detector: BaseDetector
	{
		public override TokenState Process(char current, char peek)
		{
			TokenState retval = TokenState.MORE;
			
			if (current == '\t')
			{
				retval = TokenState.IGNORE;
			}
			
			return(retval);
		}
	}

}

