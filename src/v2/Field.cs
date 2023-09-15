using System;
using System.Collections.Generic;
using System.Linq;

namespace HL7parser.v2
{
    public class Field
    {
        private string _Value;
        internal ComponentCollection ComponentList { get; set; }

        private char[] fieldDelimiters = new char[] { '^', '~', '&' };
        private bool isComponentized = false;
        private bool hasRepetitions = false;
        private List<Field> _RepetitionList;

        internal char[] FieldDelimiters
        {
            get { return fieldDelimiters; }
            set { fieldDelimiters = value; }
        }

        public bool IsComponentized
        {
            get { return isComponentized; }
            set { isComponentized = value; }
        }

        public bool HasRepetitions
        {
            get { return hasRepetitions; }
            set { hasRepetitions = value; }
        }

        internal List<Field> RepeatitionList
        {
            get
            {
                if (_RepetitionList == null)
                {
                    _RepetitionList = new List<Field>();
                }
                return _RepetitionList;
            }
            set
            {
                _RepetitionList = value;
            }
        }

        public Field()
        {
            ComponentList = new ComponentCollection();
        }

        public Field(string pValue)
        {
            ComponentList = new ComponentCollection();
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
                    hasRepetitions = _Value.Contains(FieldDelimiters[1]);

                    if (hasRepetitions)
                    {
                        _RepetitionList = new List<Field>();
                        List<string> InduvidualFields = MessageHelper.SplitString(_Value, new char[] { FieldDelimiters[1] });

                        for (int index = 0; index < InduvidualFields.Count; index++)
                        {
                            Field field = new Field
                            {
                                FieldDelimiters = new char[3] { FieldDelimiters[0], FieldDelimiters[1], FieldDelimiters[2] },
                                Value = InduvidualFields[index]
                            };
                            _RepetitionList.Add(field);
                        }
                    }
                    else
                    {
                        char[] componentSeparatorString = new char[1] { FieldDelimiters[0] };

                        List<string> AllComponents = MessageHelper.SplitString(_Value, componentSeparatorString);
                        if (AllComponents.Count > 1)
                        {
                            isComponentized = true;
                            ComponentList = new ComponentCollection();
                            foreach (string strComponent in AllComponents)
                            {
                                Component component = new Component
                                {
                                    SubComponentSeparator = new char[1] { FieldDelimiters[2] },
                                    Value = strComponent
                                };
                                ComponentList.Add(component);
                            }
                        }
                        else
                        {
                            ComponentList = new ComponentCollection();
                            Component component = new Component
                            {
                                SubComponentSeparator = new char[1] { FieldDelimiters[2] },
                                Value = _Value
                            };
                            ComponentList.Add(component);
                        }
                    }
                }
            }

        }

        public bool AddNewComponent(Component com)
        {
            try
            {
                ComponentList.Add(com);
                return true;
            }
            catch (Exception ex)
            {
                throw new HL7Exception("Unable to add new component Error - " + ex.Message);
            }
        }

        public bool AddNewComponent(Component com, int position)
        {
            try
            {
                ComponentList.Add(com, position);
                return true;
            }
            catch (Exception ex)
            {
                throw new HL7Exception("Unable to add new component Error - " + ex.Message);
            }
        }

        public Component Components(int position)
        {
            position--;
            Component com;
            try
            {
                com = ComponentList[position];
            }
            catch (Exception ex)
            {
                throw new HL7Exception("Component not availalbe Error-" + ex.Message);
            }

            return com;
        }

        public List<Component> Components()
        {
            return ComponentList;
        }

        public List<Field> Repetitions()
        {
            if (hasRepetitions)
            {
                return _RepetitionList;
            }
            return null;
        }

        public Field Repetitions(int repeatitionNumber)
        {
            if (hasRepetitions)
            {
                return _RepetitionList[repeatitionNumber - 1];
            }
            return null;
        }

    }

    internal class FieldCollection : List<Field>
    {
        internal FieldCollection()
            : base()
        {

        }

        internal new Field this[int index]
        {
            get
            {
                Field field = null;
                if (index < Count)
                    field = base[index];
                return field;
            }
            set
            {
                base[index] = value;
            }
        }

        /// <summary>
        /// add field at next position
        /// </summary>
        /// <param name="field">Field</param>
        internal new void Add(Field field)
        {
            base.Add(field);
        }

        /// <summary>
        /// Add field at specific position
        /// </summary>
        /// <param name="field">Field</param>
        /// <param name="position">position</param>
        internal void Add(Field field, int position)
        {
            int listCount = Count;

            if (position <= listCount)
                base[position] = field;
            else
            {
                for (int fieldIndex = listCount + 1; fieldIndex <= position; fieldIndex++)
                {
                    Field blankField = new Field
                    {
                        Value = string.Empty
                    };
                    base.Add(blankField);
                }
                base.Add(field);
            }
        }
    }
}