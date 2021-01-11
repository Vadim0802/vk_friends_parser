using System;
using System.Collections.Generic;

namespace VKParse
{
    public class Person
    {
        private readonly string _firstName;

        private readonly string _lastName;

        private readonly string _middleName;

        private readonly string _city;

        public readonly List<string> FriendsIds = new List<string>();
        
        public readonly string Id;

        public Person(string id, string firstName, string lastName, string middleName, string city)
        {
            Id = id;
            _firstName = firstName;
            _lastName = lastName;
            _middleName = middleName;
            _city = city;
        }

        public override string ToString()
        {
            var info = $"{Id},{_firstName},{_lastName},{_middleName},{_city},{String.Join(',', FriendsIds)}\n";

            return info;
        }
    }
}
