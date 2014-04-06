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

namespace FECT.Tokenizer.Detectors
{
	public class IdentifierKeyword_Detector : Identifier_Detector
	{
		public List<string>	KeywordList;
		public Boolean KeywordsCheckCase = false; 
		public UInt32 KeywordTokenID;
		
		public IdentifierKeyword_Detector()
		{
			KeywordList = new List<string>();
			KeywordsCheckCase = false;
			KeywordTokenID = FECT.TokenTypes.DefaultTokenType.ID;
		}
		
		public void Add(string keyword)
		{
			KeywordList.Add(keyword);
		}
		
		public void Add(string[] keyword)
		{
			foreach(string str in keyword)
			{
				Add(str);
			}
		}
		
		public override Token CreateToken (string tokenstr, TokenPosition pos)
		{
			Token retval = base.CreateToken (tokenstr, pos);
			Boolean keyword_found = false;
			
			foreach(string str in KeywordList)
			{
				if (KeywordsCheckCase == true)
				{
					if (str.ToUpper() == tokenstr.ToUpper())
					 keyword_found = true;
				}
				else if (str == tokenstr)
				{
					keyword_found = true;
				}	
				
				if (keyword_found == true)
				{
					retval.Type = FECT.TokenTypes.Find(KeywordTokenID);
					break;
				}
			}
						
			return(retval);
		}
		
	}
	
}
