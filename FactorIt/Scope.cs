using System;

namespace FactorIt
{
	[Flags]
	public enum Scope
	{
		Default		= Local,

		Upward		= Parent | Local,
		Downward	= Local | Children,
		All			= Parent | Local | Children,

		Parent		= 1 << 0,
		Local		= 1 << 1,
		Children	= 1 << 2
	}
}