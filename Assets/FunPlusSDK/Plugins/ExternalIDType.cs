using UnityEngine;
using System.Collections;

namespace FunPlus
{
	public enum ExternalIDType
	{
		InAppUserID,
		Email,
		FacebookID
	}

	static class ExternalIDTypeExtension
	{
		public static string ToTypeString(this ExternalIDType self)
		{
			switch (self)
			{
			case ExternalIDType.Email:
				return "email";
			case ExternalIDType.FacebookID:
				return "facebook_id";
			default:
				return "inapp_user_id";
			}
		}
	}
}