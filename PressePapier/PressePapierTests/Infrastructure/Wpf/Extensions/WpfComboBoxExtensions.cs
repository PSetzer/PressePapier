using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;

// ReSharper disable CheckNamespace
public static class WpfComboBoxExtensions
// ReSharper restore CheckNamespace
{
    public static void Select(this WpfComboBox listControl, Predicate<WpfListItem> shouldSelectItem)
    {
        listControl.WaitForControlEnabled();

        listControl.SelectedItem = listControl.Items.Cast<WpfListItem>().First(i => shouldSelectItem(i)).DisplayText;
    }
}