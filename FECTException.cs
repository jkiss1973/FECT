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

namespace FECT
{
	
	public enum ExceptionStength
	{
		WARNING,
		ERROR,
		FATAL,
	}
	
	[Serializable]
	public class FECTException : Exception
	{
		public FECT.Phase 		Phase;	
		public FECT.ExceptionStength	Strength;
		
		public FECTException (FECT.Phase phase, FECT.ExceptionStength strength, string message) : base(message)
		{
			Phase = phase;
			Strength = strength;
		}
	}
}

