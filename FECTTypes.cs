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

	public enum Phase
	{
		TOKENIZER,
		PARSER,
		COMPILER
	}
	
	public enum TokenAsciiCode
	{
		NUL,BAD		= 0x00,		// NULL or Bad(FALSE)
		SOH,GOOD 	= 0x01,		// Start of Header or Good(TRUE)
		EOT,EOF 	= 0x04,		// End of Tranmission but using it as end of FILE
		HT			= 0x09,		// Horizontal Tab 
		CR,EOL		= 0x0D,		// Carriage Return or End of Line 
		SPC 		= 0x20,		// Space 
	}
	
	public enum TokenState
	{
		ACCEPTED,			// Accept current stream as a valid token
		MORE,				// Keep streaming chars 
		IGNORE,				// Current stream should be ignored
		REJECTED,			// Stream is not a valid token
		FILTERED,			// Current stream is filtered.
		UNKNOWN				// Current stream is unknown, should never use this!
	}
	
	
	public delegate void FECTErrorHandler(FECT.Phase phase, UInt32 errorno, Token token,string message);
	
}

