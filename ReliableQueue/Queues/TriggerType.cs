// ************************************************************************
// *****      COPYRIGHT 2014 - 2016 HONEYWELL INTERNATIONAL SARL      *****
// ************************************************************************

namespace ReliableQueue.Queues
{
    using System.ComponentModel;

    public enum TriggerType
    {
        /// <summary>
        /// Event based trigger
        /// </summary>
        [Description("Event based trigger")]
        EventBased,
        /// <summary>
        /// Scheduled trigger
        /// </summary>
        [Description("Scheduled Trigger")]
        Scheduled
    }
}
