﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeldaOracle.Common.Scripting {
	/// <summary>An object containing a unique identifier for its type.</summary>
	public interface IIDObject {

		/// <summary>Gets the unique identifier for the object of this type.</summary>
		string ID { get; }
	}
}
