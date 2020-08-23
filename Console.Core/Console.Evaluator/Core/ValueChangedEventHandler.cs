using System;

namespace Console.Evaluator.Core
{
    /// <summary>
    /// The Event Handler for the OPCode.ValueChanged event
    /// </summary>
    /// <param name="sender">Sender of the Event</param>
    /// <param name="e">The Event Args</param>
    public delegate void ValueChangedEventHandler(object sender, EventArgs e);
}