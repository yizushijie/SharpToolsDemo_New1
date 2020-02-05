using System;
using System.Drawing;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace TestAgent.GraphicsLib
{
    /// <summary>
    /// Class that handles the data associated with a text title and its associated font
    /// properties.  Inherits from <see cref="Label" />, and adds the <see cref="Gap" />
    /// property for use by the <see cref="Axis" /> and <see cref="PaneBase" /> objects.
    /// </summary>
    /// 
    [Serializable]
    public class GapLabel : Label, ICloneable, ISerializable
    {
        #region ��������
        internal float _gap;
        #endregion

        #region ���캯��

        /// <summary>
        /// Constructor to build a <see cref="GapLabel" /> from the text and the
        /// associated font properties.
        /// </summary>
        /// <param name="text">The <see cref="string" /> representing the text to be
        /// displayed</param>
        /// <param name="fontFamily">The <see cref="String" /> font family name</param>
        /// <param name="fontSize">The size of the font in points and scaled according
        /// to the <see cref="PaneBase.CalcScaleFactor" /> logic.</param>
        /// <param name="color">The <see cref="Color" /> instance representing the color
        /// of the font</param>
        /// <param name="isBold">true for a bold font face</param>
        /// <param name="isItalic">true for an italic font face</param>
        /// <param name="isUnderline">true for an underline font face</param>
        public GapLabel(string text, string fontFamily, float fontSize, Color color, bool isBold,
                                bool isItalic, bool isUnderline)
            : base(text, fontFamily, fontSize, color, isBold, isItalic, isUnderline)
        {
            _gap = Default.Gap;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="rhs">the <see cref="AxisLabel" /> instance to be copied.</param>
        public GapLabel(GapLabel rhs)
            : base(rhs)
        {
            _gap = rhs._gap;
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
        public new GapLabel Clone()
        {
            return new GapLabel(this);
        }

        #endregion

        #region ���Զ���

        /// <summary>
        /// Gets or sets the gap factor between this label and the opposing <see cref="Axis" />
        /// or <see cref="Chart" />.
        /// </summary>
        /// <remarks>
        /// This value is expressed as a fraction of the character height for the <see cref="GapLabel" />.
        /// </remarks>
        public float Gap
        {
            get { return _gap; }
            set { _gap = value; }
        }

        /// <summary>
        /// Calculate the size of the <see cref="Gap" /> based on the <see cref="Label.FontSpec" />
        /// height, in pixel units and scaled according to <see paramref="scalefactor" />.
        /// </summary>
        /// <param name="scaleFactor">The scaling factor to be applied</param>
        public float GetScaledGap(float scaleFactor)
        {
            return _fontSpec.GetHeight(scaleFactor) * _gap;
        }

        #endregion

        #region ���л���������

        /// <summary>
        /// Current schema value that defines the version of the serialized file
        /// </summary>
        public const int schema2 = 10;

        /// <summary>
        /// Constructor for deserializing objects
        /// </summary>
        /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
        /// </param>
        /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
        /// </param>
        protected GapLabel(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // The schema value is just a file version parameter.  You can use it to make future versions
            // backwards compatible as new member variables are added to classes
            int sch2 = info.GetInt32("schema2");

            _gap = info.GetSingle("gap");
        }
        /// <summary>
        /// Populates a <see cref="SerializationInfo"/> instance with the data needed to serialize the target object
        /// </summary>
        /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data</param>
        /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data</param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("schema2", schema2);
            info.AddValue("gap", _gap);
        }
        #endregion

        #region Ĭ��ֵ����
        /// <summary>
        /// A simple struct that defines the
        /// default property values for the <see cref="GapLabel"/> class.
        /// </summary>
        public struct Default
        {
            /// <summary>
            /// The default <see cref="GapLabel.Gap" /> setting.
            /// </summary>
            public static float Gap = 0.3f;
        }
        #endregion

    }
}
