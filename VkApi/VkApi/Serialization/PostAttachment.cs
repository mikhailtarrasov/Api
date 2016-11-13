using System;
using System.Runtime.Serialization;
using VkApiDll.Serialization;

namespace VkApiDll
{
    [DataContract]
    public class PostAttachment
    {
        [DataMember(Name = "type")]
        public string Type { get; internal set; }

        // Тут должна быть переменная у которой тип - это значение строки Type

        // TODO this class
    }
}