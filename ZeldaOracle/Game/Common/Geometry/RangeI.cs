﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;

namespace ZeldaOracle.Common.Geometry {
/** <summary>
 * The int precision range between a min and a max.
 * </summary> */
public struct RangeI {

	//========== CONSTANTS ===========

	/** <summary> Returns a range positioned at (0 - 0). </summary> */
	public static RangeI Zero {
		get { return new RangeI(); }
	}
	/** <summary> Returns a range positioned at (Int.Min - Int.Max). </summary> */
	public static RangeI Full {
		get { return new RangeI(Int32.MinValue, Int32.MaxValue); }
	}

	//=========== MEMBERS ============

	/** <summary> The minimum value in the range. </summary> */
	public int Min;
	/** <summary> The maximum value in the range. </summary> */
	public int Max;

	//========= CONSTRUCTORS =========

	/** <summary> Constructs a range between the 2 values. </summary> */
	public RangeI(int min, int max) {
		this.Min	= min;
		this.Max	= max;
	}
	/** <summary> Constructs a range with a single value. </summary> */
	public RangeI(int single) {
		this.Min	= single;
		this.Max	= single;
	}
	/** <summary> Constructs a copy of the specified range. </summary> */
	public RangeI(RangeI r) {
		this.Min	= r.Min;
		this.Max	= r.Max;
	}

	//=========== GENERAL ============

	/** <summary> Outputs a string representing this range as (min - max). </summary> */
	public override string ToString() {
		return "(" + Min + ", " + Max + ")";
	}
	/** <summary> Outputs a string representing this range as (min - max). </summary> */
	public string ToString(IFormatProvider provider) {
		return "(" + Min.ToString(provider) + ", " + Max.ToString(provider) + ")";
	}
	/** <summary> Outputs a string representing this range as (min - max). </summary> */
	public string ToString(string format, IFormatProvider provider) {
		return "(" + Min.ToString(format, provider) + ", " + Max.ToString(format, provider) + ")";
	}
	/** <summary> Outputs a string representing this range as (min - max). </summary> */
	public string ToString(string format) {
		return "(" + Min.ToString(format) + ", " + Max.ToString(format) + ")";
	}
	/** <summary> Returns true if the specified range has the same min and max values. </summary> */
	public override bool Equals(object obj) {
		if (obj is RangeI)
			return (Min == ((RangeI)obj).Min && Max == ((RangeI)obj).Max);
		return false;
	}
	/** <summary> Returns the hash code for this range. </summary> */
	public override int GetHashCode() {
		return base.GetHashCode();
	}

	//========== OPERATORS ===========

	public static bool operator ==(RangeI r1, RangeI r2) {
		return (r1.Min == r2.Min && r1.Max == r2.Max);
	}
	public static bool operator ==(int i1, RangeI r2) {
		return (i1 == r2.Min && i1 == r2.Max);
	}
	public static bool operator ==(RangeI r1, int i2) {
		return (r1.Min == i2 && r1.Max == i2);
	}

	public static bool operator !=(RangeI r1, RangeI r2) {
		return (r1.Min != r2.Min || r1.Max != r2.Max);
	}
	public static bool operator !=(int i1, RangeI r2) {
		return (i1 != r2.Min || i1 != r2.Max);
	}
	public static bool operator !=(RangeI r1, int i2) {
		return (r1.Min != i2 || r1.Max != i2);
	}

	//========== PROPERTIES ==========

	/** <summary> Gets the range between the min and max values. </summary> */
	[ContentSerializerIgnore]
	public int Range {
		get { return Max - Min; }
	}
	/** <summary> Gets or sets the min or max coordinate from the index. </summary> */
	[ContentSerializerIgnore]
	public int this[int index] {
		get {
			if (index < 0 || index > 1)
				throw new System.IndexOutOfRangeException("RangeI[index] must be either 0 or 1.");
			else
				return (index == 0 ? Min : Max);
		}
		set {
			if (index < 0 || index > 1)
				throw new System.IndexOutOfRangeException("RangeI[index] must be either 0 or 1.");
			else if (index == 0)
				Min = value;
			else
				Max = value;
		}
	}
	/** <summary> Returns true if the range has the values of (0 - 0). </summary> */
	public bool IsZero {
		get { return (Min == 0 && Max == 0); }
	}
	/** <summary> Returns true if the min and max values are the same. </summary> */
	public bool IsSingle {
		get { return (Min == Max); }
	}

	//=========== CONTAINS ===========

	/** <summary> Returns true if the specified value is inside this range. </summary> */
	public bool Contains(int value) {
		return ((value >= Min) &&
				(value <=  Max));
	}

}
} // End namespace