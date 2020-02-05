using System;
using System.Text;

namespace TestAgent.GraphicsLib
{
	/// <summary>
	/// An interface to a collection class containing data
	/// that define the set of points to be displayed on the curve.
	/// </summary>
	/// <remarks>
	/// This interface is designed to allow customized data abstraction.  The default data
	/// collection class is <see cref="PointPairList" />, however, you can define your own
	/// data collection class using the <see cref="IPointList" /> interface.
	/// </remarks>
	/// <seealso cref="PointPairList" />
	/// <seealso cref="BasicArrayPointList" />
	/// 
	public interface IPointList : ICloneable
	{
		/// <summary>
		/// Indexer to access a data point by its ordinal position in the collection.
		/// </summary>
		/// <remarks>
		/// This is the standard interface that Example uses to access the data.  Although
		/// you must pass a <see cref="PointPair" /> here, your internal data storage format
		/// can be anything.
		/// </remarks>
		/// <param name="index">The ordinal position (zero-based) of the
		/// data point to be accessed.</param>
		/// <value>A <see cref="PointPair"/> object instance.</value>
		PointPair this[ int index ]  { get; }
		/// <summary>
		/// Gets the number of points available in the list.
		/// </summary>
		int Count { get; }
	}
}
