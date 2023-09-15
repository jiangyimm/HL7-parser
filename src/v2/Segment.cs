using System.Collections.Generic;
using System;

namespace HL7parser.v2
{
    public class Segment
    {
        private string _Value;
        private string _Name;
        private List<Segment> _List;
        private char[] fieldDelimiters = new char[] { '|', '^', '~', '&' };
        private short seqNo = 0;

        internal char[] FieldDelimiters
        {
            get { return fieldDelimiters; }
            set { fieldDelimiters = value; }
        }

        internal FieldCollection FieldList { get; set; }

        public Segment()
        {
            FieldList = new FieldCollection();
        }

        public Segment(string pName)
        {
            FieldList = new FieldCollection();
            _Name = pName;
        }

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        internal short SequenceNo
        {
            get
            {
                return seqNo;
            }
            set
            {
                seqNo = value;
            }
        }

        internal string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                if (_Value.Length > 0)
                {
                    char[] fieldSeparatorString = new char[1] { FieldDelimiters[0] };
                    List<string> AllFields = MessageHelper.SplitString(_Value, fieldSeparatorString);

                    if (AllFields.Count > 1)
                    {
                        if (Name == "MSH")
                        {
                            AllFields[0] = new string(fieldSeparatorString);
                        }
                        else
                            AllFields.RemoveAt(0);

                        foreach (string strField in AllFields)
                        {
                            Field field = new Field
                            {
                                FieldDelimiters = new char[3] { FieldDelimiters[1], FieldDelimiters[2], FieldDelimiters[3] },
                                Value = strField
                            };
                            FieldList.Add(field);
                        }
                    }
                    else
                    {
                        Field field = new Field
                        {
                            FieldDelimiters = new char[3] { FieldDelimiters[1], FieldDelimiters[2], FieldDelimiters[3] },
                            Value = _Value
                        };
                        FieldList.Add(field);
                    }
                }
            }

        }

        internal List<Segment> List
        {
            get
            {
                if (_List == null)
                {
                    _List = new List<Segment>();
                }
                return _List;
            }
            set
            {
                _List = value;
            }
        }

        public bool AddNewField(Field field)
        {
            try
            {
                FieldList.Add(field);
                return true;
            }
            catch (Exception ex)
            {
                throw new HL7Exception("Unable to add new field in segment " + Name + " Error - " + ex.Message);
            }
        }

        public bool AddNewField(Field field, int position)
        {
            position--;
            try
            {
                FieldList.Add(field, position);
                return true;
            }
            catch (Exception ex)
            {
                throw new HL7Exception("Unable to add new field in segment " + Name + " Error - " + ex.Message);
            }
        }

        public Field Fields(int position)
        {
            position--;
            Field field;
            try
            {
                field = FieldList[position];
            }
            catch (Exception ex)
            {
                throw new HL7Exception("Field not availalbe Error-" + ex.Message);
            }

            return field;
        }

        public List<Field> GetAllFields()
        {
            return FieldList;
        }
    }
}