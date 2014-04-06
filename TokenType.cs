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
using System.Collections.Generic;

namespace FECT
{

	public class TokenType
	{
		
		public UInt32 ID { get; set; }
		public String Name { get; set; }
		
	}
	
	public class TokenTypes
	{
		
		static public List<TokenType> TokenTypeList;
		static public TokenType DefaultTokenType;
		
		static TokenTypes()
		{
			TokenTypeList = new List<TokenType>();
		}
		
		static public void Register(TokenType type)
		{
			try
			{
				TokenTypeList.Add(type);
			}
			catch
			{
				throw new FECTException(Phase.TOKENIZER,ExceptionStength.FATAL,"Cannot register token");
			}
			
		}
		
		static public void RegisterDefault(TokenType type)
		{
			DefaultTokenType = type;
			TokenTypes.Register(type);
		}
		
		static public TokenType Find(UInt32 id)
		{
			TokenType retval = null;
			
			foreach (TokenType type in TokenTypeList)
			{
				if ( (UInt32)type.ID == id)
				{
					retval = type;
					break;
				}
			}
			
			if (retval == null)
				throw new FECTException(Phase.TOKENIZER,ExceptionStength.FATAL,string.Format("Unable to find registered Token Type {0}",id));
			
			return(retval);
		}
	}
	
}
