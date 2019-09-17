﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Telerik.Data.Core.Fields
{
    internal class ExpandoObjectFieldInfo : IDataFieldInfo, IMemberAccess
    {
        private string name;
        private Type dataType;

        public ExpandoObjectFieldInfo(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public Type DataType
        {
            get
            {
                return this.dataType;
            }
        }

        public FieldRole Role
        {
            get;
            set;
        }

        public string DisplayName
        {
            get
            {
                return this.name;
            }
        }

        public object GetValue(object item)
        {
            IDictionary<string, object> expandoObject = item as IDictionary<string, object>;
            if (expandoObject != null)
            {
                // Consider ICustomPropertyProvider + ICustomProperty
                object value;
                if (expandoObject.TryGetValue(this.name, out value))
                {
                    if (this.dataType == null && value != null)
                    {
                        // This late evaluation of DataType is potentially error-prone
                        this.dataType = value.GetType();
                    }
                }

                return value;
            }

            return null;
        }

        public void SetValue(object item, object fieldValue)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IDataFieldInfo info)
        {
            var expandoField = info as ExpandoObjectFieldInfo;
            if (expandoField == null)
            {
                return false;
            }

            if (this.name != expandoField.Name)
            {
                return false;
            }

            return this.dataType == expandoField.dataType;
        }
    }
}
