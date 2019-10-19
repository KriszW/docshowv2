using System;

namespace KilokoModelLibrary
{
    public delegate EventHandler NewItemNum(object sender, NewItemNumEventArgs args);

    public delegate EventHandler DeleteItemNum(object sender, DeleteItemNumEventArgs args);

    public class DeleteItemNumEventArgs : EventArgs
    {
    }

    public class NewItemNumEventArgs : EventArgs
    {
    }

    public class Events
    {
    }
}