using System;
using System.Collections.Generic;

namespace HL7parser.v2
{
    public class Component
    {
        private string _Value;
        internal List<SubComponent> SubComponentList { get; set; }
        private char[] subComponentSeparator = new char[1] { '&' };
        private bool isSubComponentized = false;

        internal char[] SubComponentSeparator
        {
            get { return subComponentSeparator; }
            set { subComponentSeparator = value; }
        }

        public bool IsSubComponentized
        {
            get { return isSubComponentized; }
            set { isSubComponentized = value; }
        }

        public Component()
        {
            SubComponentList = new List<SubComponent>();
        }
        public Component(string pValue)
        {
            SubComponentList = new List<SubComponent>();
            _Value = pValue;
        }

        public string Value
        {
            get
            {
                if (_Value == null)
                    return string.Empty;
                else
                    return _Value;
            }
            set
            {
                _Value = value;
                if (_Value.Length > 0)
                {
                    SubComponentList = new List<SubComponent>();
                    List<string> AllSubComponents = MessageHelper.SplitString(_Value, SubComponentSeparator);

                    if (AllSubComponents.Count > 1)
                    {
                        isSubComponentized = true;

                        foreach (string strSubComponent in AllSubComponents)
                        {
                            SubComponent subComponent = new SubComponent(strSubComponent);
                            SubComponentList.Add(subComponent);
                        }
                    }
                    else
                    {
                        SubComponentList = new List<SubComponent>();
                        SubComponent subComponent = new SubComponent(_Value);
                        SubComponentList.Add(subComponent);
                    }
                }
            }
        }

        public SubComponent SubComponents(int position)
        {
            position = position - 1;
            SubComponent sub;
            try
            {
                sub = SubComponentList[position];
            }
            catch (Exception ex)
            {
                throw new HL7Exception("SubComponent not availalbe Error-" + ex.Message);
            }

            return sub;
        }

        public List<SubComponent> SubComponents()
        {
            return SubComponentList;
        }
    }

    internal class ComponentCollection : List<Component>
    {
        internal ComponentCollection()
            : base()
        {

        }

        internal new Component this[int index]
        {
            get
            {
                Component com = null;
                if (index < Count)
                    com = base[index];
                return com;
            }
            set
            {
                base[index] = value;
            }
        }

        /// <summary>
        /// Add Component at next position
        /// </summary>
        /// <param name="com">Component</param>
        internal new void Add(Component com)
        {
            base.Add(com);
        }

        /// <summary>
        /// Add component at specific position
        /// </summary>
        /// <param name="com">Component</param>
        /// <param name="position">Position</param>
        internal void Add(Component com, int position)
        {
            position = position - 1;
            int listCount = base.Count;

            if (position <= listCount)
                base[position] = com;
            else
            {
                for (int comIndex = listCount + 1; comIndex <= position; comIndex++)
                {
                    Component blankCom = new Component
                    {
                        Value = string.Empty
                    };
                    base.Add(blankCom);
                }
                base.Add(com);
            }
        }
    }
}