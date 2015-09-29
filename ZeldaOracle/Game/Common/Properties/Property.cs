﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeldaOracle.Common.Properties {
	
	/// <summary>
	/// The possible data types for a property.
	/// </summary>
	public enum PropertyType {
		List,
		String,
		Integer,
		Float,
		Boolean
	}

	/// <summary>
	/// A proprety represents a piece of data that can be represented
	/// multiple types including lists of other properties.
	/// </summary>
	public class Property {

		// The name of the property.
		private string name;
		// The data type of the property.
		private PropertyType type;
		// The value for integer properties. Used as children count for lists, used as 0 or 1 for booleans.
		private int intValue;
		// The value for float properties.
		private float floatValue;
		// The value for string properties.
		private string stringValue;
		// If this property is part of a list of properties, then 'next' points to the next property in the list.
		private Property next;
		// If this property is a list of properties, then 'firstChild' points to the first property in the list.
		private Property firstChild;

		
		//-----------------------------------------------------------------------------
		// Constructors
		//-----------------------------------------------------------------------------

		// Construct a property with the given name and type.
		public Property(string name, PropertyType type = PropertyType.String) {
			this.name			= name;
			this.type			= type;
			this.intValue		= 0;
			this.floatValue		= 0;
			this.stringValue	= "";
			this.next			= null;
			this.firstChild		= null;
		}
		
		// Construct a property as a copy of another.
		public Property(Property copy) {
			name			= copy.name;
			type			= copy.type;
			intValue		= copy.intValue;
			floatValue		= copy.floatValue;
			stringValue		= copy.stringValue;
			next			= null;
			firstChild		= null;

			if (copy.firstChild != null)
				firstChild = new Property(copy.firstChild);
			if (copy.next != null)
				next = new Property(copy.next);
		}


		//-----------------------------------------------------------------------------
		// Accessors
		//-----------------------------------------------------------------------------

		public bool EqualsValue(Property other) {
			if (type != other.type || type == PropertyType.List)
				return false;
			else if (type == PropertyType.String)
				return (StringValue == other.StringValue);
			else if (type == PropertyType.Integer)
				return (IntValue == other.IntValue);
			else if (type == PropertyType.Float)
				return (FloatValue == other.FloatValue);
			else if (type == PropertyType.Boolean)
				return (BoolValue == other.BoolValue);
			return false;
		}

		// Get the child at the given index (if this is a list).
		public Property GetChild(int index) {
			if (type != PropertyType.List)
				return null;

			Property p = firstChild;
			int i = 0;
			while (p != null) {
				if (i == index)
					return p;
				p = p.next;
				i++;
			}

			return null; // ERROR: Index out of bounds!
		}


		//-----------------------------------------------------------------------------
		// Static methods
		//-----------------------------------------------------------------------------
		
		// Create a list of properties.
		public static Property CreateList(string name, params Property[] properties) {
			Property parent = new Property(name, PropertyType.List);
			parent.Count = properties.Length;

			if (properties.Length > 0) {
				Property p = new Property(properties[0]);
				parent.firstChild = p;
				for (int i = 0; i < properties.Length - 1; i++) {
					p.next = new Property(properties[i + 1]);
					p = p.next;
				}
			}
			return parent;
		}
		
		// Create a string property with the given value.
		public static Property CreateString(string name, string value) {
			Property p = new Property(name, PropertyType.String);
			p.StringValue = value;
			return p;
		}
		
		// Create an integer property with the given value.
		public static Property CreateInt(string name, int value) {
			Property p = new Property(name, PropertyType.Integer);
			p.IntValue = value;
			return p;
		}
		
		// Create a float property with the given value.
		public static Property CreateFloat(string name, float value) {
			Property p = new Property(name, PropertyType.Float);
			p.FloatValue = value;
			return p;
		}
		
		// Create a boolean property with the given value.
		public static Property CreateBool(string name, bool value) {
			Property p = new Property(name, PropertyType.Boolean);
			p.BoolValue = value;
			return p;
		}


		//-----------------------------------------------------------------------------
		// Properties
		//-----------------------------------------------------------------------------

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public PropertyType Type {
			get { return type; }
			set { type = value; }
		}

		// Values.

		public string StringValue {
			get { return stringValue; }
			set { stringValue = value; }
		}

		public int IntValue {
			get { return intValue; }
			set { intValue = value; }
		}

		public float FloatValue {
			get { return floatValue; }
			set { floatValue = value; }
		}

		public bool BoolValue {
			get { return (intValue == 1); }
			set { intValue = (value ? 1 : 0); }
		}

		// Lists.

		public Property Next {
			get { return next; }
			set { next = value; }
		}

		public Property FirstChild {
			get { return firstChild; }
			set { firstChild = value; }
		}

		// Return the child at the given index.
		public Property this[int index] {
			get { return GetChild(index); }
		}

		// Return the number of children
		public int Count {
			get { return intValue; }
			set { intValue = value; }
		}
	}
}