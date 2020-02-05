using System;
using System.Drawing;
using System.Collections.Generic;

namespace TestAgent.GraphicsLib
{
    /// <summary>
    /// A collection class containing a list of <see cref="PointPair"/> objects
    /// that define the set of points to be displayed on the curve.
    /// </summary>
    /// <seealso cref="BasicArrayPointList" />
    /// <seealso cref="IPointList" />
    /// 
    [Serializable]
    public class PointPairList : List<PointPair>, IPointList, IPointListEdit
    {
        #region ��������
        /// <summary>�洢����״̬�ı���
        /// Private field to maintain the sort status of this
        /// <see cref="PointPairList"/>.  Use the public property
        /// <see cref="Sorted"/> to access this value.
        /// </summary>
        protected bool _sorted = true;
        #endregion

        #region ���Զ���

        /// <summary>
        /// true if the list is currently sorted.
        /// </summary>
        /// <seealso cref="Sort()"/>
        public bool Sorted
        {
            get { return _sorted; }
        }
        #endregion

        #region ���캯��
        /// <summary>
        /// Default constructor for the collection class
        /// </summary>
        public PointPairList()
        {
            _sorted = false;
        }

        /// <summary>
        /// Constructor to initialize the PointPairList from two arrays of
        /// type double.
        /// </summary>
        public PointPairList(double[] x, double[] y)
        {
            this.Add(x, y);

            _sorted = false;
        }

        /// <summary>
        /// �ø�����Point���鹹������
        /// </summary>
        /// <param name="points"></param>
        public PointPairList(PointF[] points)
        {
            this.Add(points);
            _sorted = false;
        }

        /// <summary>
        /// �ø�����PointF���鹹������
        /// </summary>
        /// <param name="points"></param>
        public PointPairList(Point[] points)
        {
            this.Add(points);
            _sorted = false;
        }

        /// <summary>
        /// Constructor to initialize the PointPairList from an IPointList
        /// </summary>
        public PointPairList(IPointList list)
        {
            int count = list.Count;
            for (int i = 0; i < count; i++)
                Add(list[i]);

            _sorted = false;
        }

        /// <summary>
        /// Constructor to initialize the PointPairList from three arrays of
        /// type double.
        /// </summary>
        public PointPairList(double[] x, double[] y, double[] baseVal)
        {
            Add(x, y, baseVal);

            _sorted = false;
        }

        /// <summary>
        /// The Copy Constructor
        /// </summary>
        /// <param name="rhs">The PointPairList from which to copy</param>
        public PointPairList(PointPairList rhs)
        {
            Add(rhs);

            _sorted = false;
        }

        /// <summary>
        /// Implement the <see cref="ICloneable" /> interface in a typesafe manner by just
        /// calling the typed version of <see cref="Clone" />
        /// </summary>
        /// <returns>A deep copy of this object</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Typesafe, deep-copy clone method.
        /// </summary>
        /// <returns>A new, independent copy of this class</returns>
        public PointPairList Clone()
        {
            return new PointPairList(this);
        }

        #endregion

        #region ��������
        /// <summary>
        /// Add a <see cref="PointPair"/> object to the collection at the end of the list.
        /// </summary>
        /// <param name="point">The <see cref="PointPair"/> object to
        /// be added</param>
        /// <returns>The zero-based ordinal index where the point was added in the list.</returns>
        public new void Add(PointPair point)
        {
            _sorted = false;
            //base.Add( new PointPair( point ) );
            base.Add(point.Clone());
        }

        /// <summary>
        /// Add a <see cref="PointPairList"/> object to the collection at the end of the list.
        /// </summary>
        /// <param name="pointList">A reference to the <see cref="PointPairList"/> object to
        /// be added</param>
        /// <returns>The zero-based ordinal index where the last point was added in the list,
        /// or -1 if no points were added.</returns>
        public void Add(PointPairList pointList)
        {
            foreach (PointPair point in pointList)
                Add(point);

            _sorted = false;
        }

        /// <summary>
        /// ����һ��Point������Ϊ����
        /// </summary>
        /// <param name="points">Point����</param>
        public void Add(Point[] points)
        {
            if (points != null)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    PointPair point = new PointPair(new PointF(points[i].X, points[i].Y));
                    base.Add(point);
                }
            }
            _sorted = false;
        }

        /// <summary>
        /// ��PointF������Ϊ��������
        /// </summary>
        /// <param name="points">PointF����</param>
        public void Add(PointF[] points)
        {
            if (points != null)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    PointPair point = new PointPair(points[i]);
                    base.Add(point);
                }
            }
            _sorted = false;
        }

        /// <summary>
        /// Add a set of points to the PointPairList from two arrays of type double.
        /// If either array is null, then a set of ordinal values is automatically
        /// generated in its place (see <see cref="AxisType.Ordinal"/>.
        /// If the arrays are of different size, then the larger array prevails and the
        /// smaller array is padded with <see cref="PointPairBase.Missing"/> values.
        /// </summary>
        /// <param name="x">A double[] array of X values</param>
        /// <param name="y">A double[] array of Y values</param>
        /// <returns>The zero-based ordinal index where the last point was added in the list,
        /// or -1 if no points were added.</returns>
        public void Add(double[] x, double[] y)
        {
            int len = 0;

            if (x != null)
                len = x.Length;
            if (y != null && y.Length > len)
                len = y.Length;

            for (int i = 0; i < len; i++)
            {
                PointPair point = new PointPair(0, 0, 0);
                if (x == null)
                    point.X = (double)i + 1.0;
                else if (i < x.Length)
                    point.X = x[i];
                else
                    point.X = PointPair.Missing;

                if (y == null)
                    point.Y = (double)i + 1.0;
                else if (i < y.Length)
                    point.Y = y[i];
                else
                    point.Y = PointPair.Missing;

                base.Add(point);
            }

            _sorted = false;
        }

        /// <summary>
        /// Add a set of points to the <see cref="PointPairList"/> from three arrays of type double.
        /// If the X or Y array is null, then a set of ordinal values is automatically
        /// generated in its place (see <see cref="AxisType.Ordinal"/>.  If the <see paramref="baseVal"/>
        /// is null, then it is set to zero.
        /// If the arrays are of different size, then the larger array prevails and the
        /// smaller array is padded with <see cref="PointPairBase.Missing"/> values.
        /// </summary>
        /// <param name="x">A double[] array of X values</param>
        /// <param name="y">A double[] array of Y values</param>
        /// <param name="z">A double[] array of Z or lower-dependent axis values</param>
        /// <returns>The zero-based ordinal index where the last point was added in the list,
        /// or -1 if no points were added.</returns>
        public void Add(double[] x, double[] y, double[] z)
        {
            int len = 0;

            if (x != null)
                len = x.Length;
            if (y != null && y.Length > len)
                len = y.Length;
            if (z != null && z.Length > len)
                len = z.Length;

            for (int i = 0; i < len; i++)
            {
                PointPair point = new PointPair();

                if (x == null)
                    point.X = (double)i + 1.0;
                else if (i < x.Length)
                    point.X = x[i];
                else
                    point.X = PointPair.Missing;

                if (y == null)
                    point.Y = (double)i + 1.0;
                else if (i < y.Length)
                    point.Y = y[i];
                else
                    point.Y = PointPair.Missing;

                if (z == null)
                    point.Z = (double)i + 1.0;
                else if (i < z.Length)
                    point.Z = z[i];
                else
                    point.Z = PointPair.Missing;

                base.Add(point);
            }

            _sorted = false;
        }

        /// <summary>
        /// ����Ϊ����������
        /// </summary>
        /// <param name="pointList"></param>
        public void UpdateWith(PointPairList pointList)
        {
            base.Clear();
            this.Add(pointList);
        }

        /// <summary>
        /// ��������Ϊ������Pont����
        /// </summary>
        /// <param name="points"></param>
        public void UpdateWith(Point[] points)
        {
            base.Clear();
            this.Add(points);
        }

        /// <summary>
        /// ��������Ϊ������PointF����
        /// </summary>
        /// <param name="points"></param>
        public void UpdateWith(PointF[] points)
        {
            base.Clear();
            this.Add(points);
        }

        /// <summary>
        /// ����Ϊ������X��Y����
        /// </summary>
        /// <param name="x">X�������</param>
        /// <param name="y">Y�������</param>
        public void UpdateWith(double[] x, double[] y)
        {
            base.Clear();
            this.Add(x, y);
        }

        /// <summary>
        /// ����Ϊ������X��Y��Z����
        /// </summary>
        /// <param name="x">X�������</param>
        /// <param name="y">Y�������</param>
        /// <param name="z">Z�������</param>
        public void UpdateWith(double[] x, double[] y, double[] z)
        {
            base.Clear();
            this.Add(x, y, z);
        }

        /// <summary>
        /// Add a single point to the <see cref="PointPairList"/> from values of type double.
        /// </summary>
        /// <param name="x">The X value</param>
        /// <param name="y">The Y value</param>
        /// <returns>The zero-based ordinal index where the point was added in the list.</returns>
        public void Add(double x, double y)
        {
            _sorted = false;
            PointPair point = new PointPair(x, y);
            base.Add(point);
        }

        /// <summary>
        /// Add a single point to the <see cref="PointPairList"/> from values of type double.
        /// </summary>
        /// <param name="x">The X value</param>
        /// <param name="y">The Y value</param>
        /// <param name="tag">The Tag value for the PointPair</param>
        /// <returns>The zero-based ordinal index where the point was added in the list.</returns>
        public void Add(double x, double y, string tag)
        {
            _sorted = false;
            PointPair point = new PointPair(x, y, tag);
            base.Add(point);
        }

        /// <summary>
        /// Add a single point to the <see cref="PointPairList"/> from values of type double.
        /// </summary>
        /// <param name="x">The X value</param>
        /// <param name="y">The Y value</param>
        /// <param name="z">The Z or lower dependent axis value</param>
        /// <returns>The zero-based ordinal index where the point was added
        /// in the list.</returns>
        public void Add(double x, double y, double z)
        {
            _sorted = false;
            PointPair point = new PointPair(x, y, z);
            base.Add(point);
        }

        /// <summary>
        /// Add a single point to the <see cref="PointPairList"/> from values of type double.
        /// </summary>
        /// <param name="x">The X value</param>
        /// <param name="y">The Y value</param>
        /// <param name="z">The Z or lower dependent axis value</param>
        /// <param name="tag">The Tag value for the PointPair</param>
        /// <returns>The zero-based ordinal index where the point was added
        /// in the list.</returns>
        public void Add(double x, double y, double z, string tag)
        {
            _sorted = false;
            PointPair point = new PointPair(x, y, z, tag);
            base.Add(point);
        }

        /// <summary>
        /// Add a <see cref="PointPair"/> object to the collection at the specified,
        /// zero-based, index location.
        /// </summary>
        /// <param name="index">
        /// The zero-based ordinal index where the point is to be added in the list.
        /// </param>
        /// <param name="point">
        /// The <see cref="PointPair"/> object to be added.
        /// </param>
        public new void Insert(int index, PointPair point)
        {
            _sorted = false;
            base.Insert(index, point);
        }

        /// <summary>
        /// �ڸ�����λ�ò���һ��Point��
        /// </summary>
        /// <param name="index"></param>
        /// <param name="point"></param>
        public void Insert(int index, Point point)
        {
            _sorted = false;
            base.Insert(index, new PointPair(new PointF(point.X, point.Y)));
        }

        /// <summary>
        /// �ڸ�����λ�ò���һ��PointF��
        /// </summary>
        /// <param name="index"></param>
        /// <param name="point"></param>
        public void Insert(int index, PointF point)
        {
            _sorted = false;
            base.Insert(index, new PointPair(point));
        }

        /// <summary>
        /// Add a single point (from values of type double ) to the <see cref="PointPairList"/> at the specified,
        /// zero-based, index location.
        /// </summary>
        /// <param name="index">
        /// The zero-based ordinal index where the point is to be added in the list.
        /// </param>
        /// <param name="x">The X value</param>
        /// <param name="y">The Y value</param>
        public void Insert(int index, double x, double y)
        {
            _sorted = false;
            base.Insert(index, new PointPair(x, y));
        }

        /// <summary>
        /// Add a single point (from values of type double ) to the <see cref="PointPairList"/> at the specified,
        /// zero-based, index location.
        /// </summary>
        /// <param name="index">
        /// The zero-based ordinal index where the point is to be added in the list.
        /// </param>
        /// <param name="x">The X value</param>
        /// <param name="y">The Y value</param>
        /// <param name="z">The Z or lower dependent axis value</param>
        public void Insert(int index, double x, double y, double z)
        {
            _sorted = false;
            Insert(index, new PointPair(x, y, z));
        }

        /// <summary>
        /// Return the zero-based position index of the
        /// <see cref="PointPair"/> with the specified label <see cref="PointPair.Tag"/>.
        /// </summary>
        /// <remarks>The <see cref="PointPair.Tag"/> object must be of type <see cref="String"/>
        /// for this method to find it.</remarks>
        /// <param name="label">The <see cref="String"/> label that is in the
        /// <see cref="PointPair.Tag"/> attribute of the item to be found.
        /// </param>
        /// <returns>The zero-based index of the specified <see cref="PointPair"/>,
        /// or -1 if the <see cref="PointPair"/> is not in the list</returns>
        public int IndexOfTag(string label)
        {
            int iPt = 0;
            foreach (PointPair p in this)
            {
                if (p.Tag is string && String.Compare((string)p.Tag, label, true) == 0)
                    return iPt;
                iPt++;
            }

            return -1;
        }

        /// <summary>
        /// Compare two <see cref="PointPairList"/> objects to see if they are equal.
        /// </summary>
        /// <remarks>Equality is based on equal count of <see cref="PointPair"/> items, and
        /// each individual <see cref="PointPair"/> must be equal (as per the
        /// <see cref="PointPair.Equals"/> method.</remarks>
        /// <param name="obj">The <see cref="PointPairList"/> to be compared with for equality.</param>
        /// <returns>true if the <see cref="PointPairList"/> objects are equal, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            PointPairList rhs = obj as PointPairList;
            if (this.Count != rhs.Count)
                return false;

            for (int i = 0; i < this.Count; i++)
            {
                if (!this[i].Equals(rhs[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Return the HashCode from the base class.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Sorts the list according to the point x values. Will not sort the 
        /// list if the list is already sorted.
        /// </summary>
        /// <returns>If the list was sorted before sort was called</returns>
        public new bool Sort()
        {
            // if it is already sorted we don't have to sort again
            if (_sorted)
                return true;

            Sort(new PointPair.PointPairComparer(SortType.XValues));
            return false;
        }

        /// <summary>
        /// Sorts the list according to the point values . Will not sort the 
        /// list if the list is already sorted.
        /// </summary>
        /// <param name="type"></param>  The <see cref = "SortType"/>
        ///used to determine whether the X or Y values will be used to sort
        ///the list
        /// <returns>If the list was sorted before sort was called</returns>
        public bool Sort(SortType type)
        {
            // if it is already sorted we don't have to sort again
            if (_sorted)
                return true;

            this.Sort(new PointPair.PointPairComparer(type));

            return false;
        }

        /// <summary>
        /// Set the X values for this <see cref="PointPairList"/> from the specified
        /// array of double values.
        /// </summary>
        /// <remarks>
        /// If <see paramref="x"/> has more values than
        /// this list, then the extra values will be ignored.  If <see paramref="x"/>
        /// has less values, then the corresponding <see cref="PointPairList"/> values
        /// will not be changed.  That is, if the <see cref="PointPairList"/> has 20 values
        /// and <see paramref="x"/> has 15 values, then the first 15 values of the
        /// <see cref="PointPairList"/> will be changed, and the last 5 values will not be
        /// changed.
        /// </remarks>
        /// <param name="x">An array of double values that will replace the existing X
        /// values in the <see cref="PointPairList"/>.</param>
        public void SetX(double[] x)
        {
            for (int i = 0; i < x.Length; i++)
            {
                if (i < this.Count)
                    this[i].X = x[i];
            }

            _sorted = false;
        }

        /// <summary>
        /// Set the Y values for this <see cref="PointPairList"/> from the specified
        /// array of double values.
        /// </summary>
        /// <remarks>
        /// If <see paramref="y"/> has more values than
        /// this list, then the extra values will be ignored.  If <see paramref="y"/>
        /// has less values, then the corresponding <see cref="PointPairList"/> values
        /// will not be changed.  That is, if the <see cref="PointPairList"/> has 20 values
        /// and <see paramref="y"/> has 15 values, then the first 15 values of the
        /// <see cref="PointPairList"/> will be changed, and the last 5 values will not be
        /// changed.
        /// </remarks>
        /// <param name="y">An array of double values that will replace the existing Y
        /// values in the <see cref="PointPairList"/>.</param>
        public void SetY(double[] y)
        {
            for (int i = 0; i < y.Length; i++)
            {
                if (i < this.Count)
                    this[i].Y = y[i];
            }

            _sorted = false;
        }

        /// <summary>
        /// Set the Z values for this <see cref="PointPairList"/> from the specified
        /// array of double values.
        /// </summary>
        /// <remarks>
        /// If <see paramref="z"/> has more values than
        /// this list, then the extra values will be ignored.  If <see paramref="z"/>
        /// has less values, then the corresponding <see cref="PointPairList"/> values
        /// will not be changed.  That is, if the <see cref="PointPairList"/> has 20 values
        /// and <see paramref="z"/> has 15 values, then the first 15 values of the
        /// <see cref="PointPairList"/> will be changed, and the last 5 values will not be
        /// changed.
        /// </remarks>
        /// <param name="z">An array of double values that will replace the existing Z
        /// values in the <see cref="PointPairList"/>.</param>
        public void SetZ(double[] z)
        {
            for (int i = 0; i < z.Length; i++)
            {
                if (i < this.Count)
                    this[i].Z = z[i];
            }

            _sorted = false;
        }

        /// <summary>
        /// Add the Y values from the specified <see cref="PointPairList"/> object to this
        /// <see cref="PointPairList"/>.  If <see paramref="sumList"/> has more values than
        /// this list, then the extra values will be ignored.  If <see paramref="sumList"/>
        /// has less values, the missing values are assumed to be zero.
        /// </summary>
        /// <param name="sumList">A reference to the <see cref="PointPairList"/> object to
        /// be summed into the this <see cref="PointPairList"/>.</param>
        public void SumY(PointPairList sumList)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (i < sumList.Count)
                    this[i].Y += sumList[i].Y;
            }

            //sorted = false;
        }

        /// <summary>
        /// Add the X values from the specified <see cref="PointPairList"/> object to this
        /// <see cref="PointPairList"/>.  If <see paramref="sumList"/> has more values than
        /// this list, then the extra values will be ignored.  If <see paramref="sumList"/>
        /// has less values, the missing values are assumed to be zero.
        /// </summary>
        /// <param name="sumList">A reference to the <see cref="PointPairList"/> object to
        /// be summed into the this <see cref="PointPairList"/>.</param>
        public void SumX(PointPairList sumList)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (i < sumList.Count)
                    this[i].X += sumList[i].X;
            }

            _sorted = false;
        }

        /// <summary>
        /// Linearly interpolate the data to find an arbitraty Y value that corresponds to the specified X value.
        /// </summary>
        /// <remarks>
        /// This method uses linear interpolation with a binary search algorithm.  It therefore
        /// requires that the x data be monotonically increasing.  Missing values are not allowed.  This
        /// method will extrapolate outside the range of the PointPairList if necessary.
        /// </remarks>
        /// <param name="xTarget">The target X value on which to interpolate</param>
        /// <returns>The Y value that corresponds to the <see paramref="xTarget"/> value.</returns>
        public double InterpolateX(double xTarget)
        {
            int lo, mid, hi;
            if (this.Count < 2)
                throw new Exception("Error: Not enough points in curve to interpolate");

            if (xTarget <= this[0].X)
            {
                lo = 0;
                hi = 1;
            }
            else if (xTarget >= this[this.Count - 1].X)
            {
                lo = this.Count - 2;
                hi = this.Count - 1;
            }
            else
            {
                // if x is within the bounds of the x table, then do a binary search
                // in the x table to find table entries that bound the x value
                lo = 0;
                hi = this.Count - 1;

                // limit to 1000 loops to avoid an infinite loop problem
                int j;
                for (j = 0; j < 1000 && hi > lo + 1; j++)
                {
                    mid = (hi + lo) / 2;
                    if (xTarget > this[mid].X)
                        lo = mid;
                    else
                        hi = mid;
                }

                if (j >= 1000)
                    throw new Exception("Error: Infinite loop in interpolation");
            }

            return (xTarget - this[lo].X) / (this[hi].X - this[lo].X) *
                    (this[hi].Y - this[lo].Y) + this[lo].Y;

        }

        /// <summary>
        /// Linearly interpolate the data to find an arbitraty X value that corresponds to the specified Y value.
        /// </summary>
        /// <remarks>
        /// This method uses linear interpolation with a binary search algorithm.  It therefore
        /// requires that the Y data be monotonically increasing.  Missing values are not allowed.  This
        /// method will extrapolate outside the range of the PointPairList if necessary.
        /// </remarks>
        /// <param name="yTarget">The target Y value on which to interpolate</param>
        /// <returns>The X value that corresponds to the <see paramref="yTarget"/> value.</returns>
        public double InterpolateY(double yTarget)
        {
            int lo, mid, hi;
            if (this.Count < 2)
                throw new Exception("Error: Not enough points in curve to interpolate");

            if (yTarget <= this[0].Y)
            {
                lo = 0;
                hi = 1;
            }
            else if (yTarget >= this[this.Count - 1].Y)
            {
                lo = this.Count - 2;
                hi = this.Count - 1;
            }
            else
            {
                // if y is within the bounds of the y table, then do a binary search
                // in the y table to find table entries that bound the y value
                lo = 0;
                hi = this.Count - 1;

                // limit to 1000 loops to avoid an infinite loop problem
                int j;
                for (j = 0; j < 1000 && hi > lo + 1; j++)
                {
                    mid = (hi + lo) / 2;
                    if (yTarget > this[mid].Y)
                        lo = mid;
                    else
                        hi = mid;
                }

                if (j >= 1000)
                    throw new Exception("Error: Infinite loop in interpolation");
            }

            return (yTarget - this[lo].Y) / (this[hi].Y - this[lo].Y) *
                    (this[hi].X - this[lo].X) + this[lo].X;

        }

        /// <summary>
        /// Use linear regression to form a least squares fit of an existing
        /// <see cref="IPointList"/> instance.
        /// </summary>
        /// <remarks>The output <see cref="PointPairList" /> will cover the
        /// same X range of data as the original dataset.
        /// </remarks>
        /// <param name="points">An <see cref="IPointList" /> instance containing
        /// the data to be regressed.</param>
        /// <param name="pointCount">The number of desired points to be included
        /// in the resultant <see cref="PointPairList" />.
        /// </param>
        /// <returns>A new <see cref="PointPairList" /> containing the resultant
        /// data fit.
        /// </returns>
        public PointPairList LinearRegression(IPointList points, int pointCount)
        {
            double minX = double.MaxValue;
            double maxX = double.MinValue;

            for (int i = 0; i < points.Count; i++)
            {
                PointPair pt = points[i];

                if (!pt.IsInvalid)
                {
                    minX = pt.X < minX ? pt.X : minX;
                    maxX = pt.X > maxX ? pt.X : maxX;
                }
            }

            return LinearRegression(points, pointCount, minX, maxX);
        }

        /// <summary>
        /// Use linear regression to form a least squares fit of an existing
        /// <see cref="IPointList"/> instance.
        /// </summary>
        /// <param name="points">An <see cref="IPointList" /> instance containing
        /// the data to be regressed.</param>
        /// <param name="pointCount">The number of desired points to be included
        /// in the resultant <see cref="PointPairList" />.
        /// </param>
        /// <param name="minX">The minimum X value of the resultant
        /// <see cref="PointPairList" />.</param>
        /// <param name="maxX">The maximum X value of the resultant
        /// <see cref="PointPairList" />.</param>
        /// <returns>A new <see cref="PointPairList" /> containing the resultant
        /// data fit.
        /// </returns>
        public PointPairList LinearRegression(IPointList points, int pointCount,
            double minX, double maxX)
        {
            double x = 0, y = 0, xx = 0, xy = 0, count = 0;
            for (int i = 0; i < points.Count; i++)
            {
                PointPair pt = points[i];
                if (!pt.IsInvalid)
                {
                    x += points[i].X;
                    y += points[i].Y;
                    xx += points[i].X * points[i].X;
                    xy += points[i].X * points[i].Y;
                    count++;
                }
            }

            if (count < 2 || maxX - minX < 1e-20)
                return null;

            double slope = (count * xy - x * y) / (count * xx - x * x);
            double intercept = (y - slope * x) / count;

            PointPairList newPoints = new PointPairList();
            double stepSize = (maxX - minX) / pointCount;
            double value = minX;
            for (int i = 0; i < pointCount; i++)
            {
                newPoints.Add(new PointPair(value, value * slope + intercept));
                value += stepSize;
            }

            return newPoints;
        }
        #endregion

    }
}


