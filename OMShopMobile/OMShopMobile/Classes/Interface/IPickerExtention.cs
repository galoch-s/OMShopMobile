using System;
using System.Collections.Generic;

namespace OMShopMobile
{
	public interface IPickerExtention
	{
		string Title { get; set; }

		IList<string> Items { get; }

		int SelectedIndex { get; set; }

		event EventHandler SelectedIndexChanged;
	}
}

