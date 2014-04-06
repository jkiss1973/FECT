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
using FECT.Tokenizer;

namespace FECT
{
	public class Token
	{
		public TokenState State {get; set; }
		public string Value { get; set; }
		public TokenPosition Position {get ; set; }
		public TokenType Type { get; set; }
		
		public override string ToString ()
		{
			return string.Format ("[Token: State={0}, Token Type={1}, LN={2},{3}] '{4}' ", State, Type.Name, Position.LineNumber, Position.CharStartPosition, Value);
		}

	}
	
	public class TokenPosition
	{
		public UInt32 LineNumber	{ get; set; }
		public UInt32 CharStartPosition {get; set; }
	}

}
