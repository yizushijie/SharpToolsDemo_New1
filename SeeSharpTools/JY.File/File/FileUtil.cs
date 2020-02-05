﻿using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using SeeSharpTools.JY.File.Common;
using SeeSharpTools.JY.File.Common.i18n;
using SeeSharpTools.JY.File.Convertor;

namespace SeeSharpTools.JY.File
{

    /// <summary>
    /// 文件存在时的写入模式
    /// Write mode when file exist
    /// </summary>
    public enum WriteMode
    {
        /// <summary>
        /// 文件存在时续写
        /// Append to file
        /// </summary>
        Append,

        /// <summary>
        /// 文件存在时覆盖
        /// Overlap file
        /// </summary>
        OverLap
    }

    /// <summary>
    /// 文件操作工具类
    /// File Read And Write Handler
    /// </summary>
    internal class FileUtil
    {
        private FileUtil()
        {
            throw new NotSupportedException("Class should not be instantiationed.");
        }

        private static I18nEntity i18n = I18nEntity.GetInstance(I18nLocalWrapper.Name);

        internal static void CheckFilePath(string filePath, string fileExtName)
        {
            if (!filePath.EndsWith($".{fileExtName}", true, CultureInfo.CurrentCulture))
            {
                throw new SeeSharpFileException(SeeSharpFileErrorCode.ParamCheckError,
                    i18n.GetStr("ParamCheck.InvalidFileType"));
            }
        }

        private static readonly string NewLineDelim = Environment.NewLine;
        internal static string BuildStringData(IEnumerator enumerator, int rowCount, int colCount,
            string delims)
        {
            StringBuilder strBuffer = new StringBuilder();

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    enumerator.MoveNext();
                    object strElem = enumerator.Current;
                    if (null == strElem || strElem.ToString().Contains(delims))
                    {
                        throw new SeeSharpFileException(SeeSharpFileErrorCode.ParamCheckError,
                            i18n.GetFStr("ParamCheck.InvalidData", delims));
                    }
                    strBuffer.Append(strElem).Append(delims);

                }
                strBuffer.Remove(strBuffer.Length - delims.Length, delims.Length);
                strBuffer.Append(NewLineDelim);
            }
            strBuffer.Remove(strBuffer.Length - NewLineDelim.Length, NewLineDelim.Length);
            return strBuffer.ToString();
        }

        internal static void CheckDataSize<TDataType>(TDataType[,] data)
        {
            if (null == data || 0 == data.GetLength(0) || 0 == data.GetLength(1))
            {
                throw new SeeSharpFileException(SeeSharpFileErrorCode.ParamCheckError,
                    i18n.GetStr("ParamCheck.WriteEmptyData"));
            }
            if (ReferenceEquals(typeof(string), typeof(TDataType)) && data.GetLength(0) == 1 && data.GetLength(1) == 1 &&
                (null == data[0, 0] || data[0, 0].Equals("")))
            {
                throw new SeeSharpFileException(SeeSharpFileErrorCode.ParamCheckError,
                    i18n.GetStr("ParamCheck.WriteEmptyData"));
            }
        }

        internal static void CheckDataSize<TDataType>(TDataType[] data)
        {
            if (null == data || 0 == data.Length)
            {
                throw new SeeSharpFileException(SeeSharpFileErrorCode.ParamCheckError,
                    i18n.GetStr("ParamCheck.WriteEmptyData"));
            }
            if (ReferenceEquals(typeof(string), typeof(TDataType)) && data.GetLength(0) == 1 && data.GetLength(1) == 1 &&
                (null == data[0] || data[0].Equals("")))
            {
                throw new SeeSharpFileException(SeeSharpFileErrorCode.ParamCheckError,
                    i18n.GetStr("ParamCheck.WriteEmptyData"));
            }
        }

        internal static string GetSaveFilePathFromDialog(string fileExtName)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = DefaultPath;
            saveFileDialog.Title = i18n.GetStr("Const.SelectWriteFile");
            saveFileDialog.Filter = string.Format("{0} FILE(*.{1})|*.{1}", fileExtName.ToUpper(), fileExtName);
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                return saveFileDialog.FileName;
            }
            else
            {
                throw new SeeSharpFileException(SeeSharpFileErrorCode.ParamCheckError,
                    i18n.GetStr("ParamCheck.SelectNoFile"));
            }
        }

        private const string DefaultPath = @"c:\user\documents";

        internal static string GetOpenFilePathFromDialog(string fileExtName)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = DefaultPath;
            openFileDialog.Title = i18n.GetStr("Const.SelectReadFile");
            openFileDialog.Filter = string.Format("{0} FILE(*.{1})|*.{1}", fileExtName.ToUpper(), fileExtName);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            else
            {
                throw new SeeSharpFileException(SeeSharpFileErrorCode.ParamCheckError,
                    i18n.GetStr("ParamCheck.SelectNoFile"));
            }
        }

        internal static string[,] GetStrData(string[] fileDatas, string delims)
        {
            int lineNum = fileDatas.Length;
            if (0 == lineNum || 0 == fileDatas.Length)
            {
                return null;
            }
            int colNum = fileDatas[0].Split(delims.ToCharArray()).Length;
            string[,] strData = new string[lineNum, colNum];
            string[] singleRowDatas = null;
            for (int rowIndex = 0; rowIndex < fileDatas.Length; rowIndex++)
            {
                singleRowDatas = fileDatas[rowIndex].Split(delims.ToCharArray());
                CheckColumnCount(colNum, singleRowDatas.Length, rowIndex);
                for (int colIndex = 0; colIndex < singleRowDatas.Length; colIndex++)
                {
                    strData[rowIndex, colIndex] = singleRowDatas[colIndex];
                }
            }
            return strData;
        }

        internal static void CheckColumnCount(int colNum, int rowColNum, int rowIndex)
        {
            if (colNum != rowColNum)
            {
                throw new SeeSharpFileException(SeeSharpFileErrorCode.ParamCheckError,
                    i18n.GetFStr("ParamCheck.ColCountNotFit", rowIndex + 1));
            }
        }

        internal static void CheckTemplateType<TDataType>(Type type)
        {
            if (!ReferenceEquals(type, typeof(TDataType)))
            {
                throw new SeeSharpFileException(SeeSharpFileErrorCode.ParamCheckError,
                    i18n.GetFStr("ParamCheck.InvalidOperationInType", type.Name));
            }
        }

        internal static byte[] BuildByteData<TDataType>(TDataType[,] data)
        {
            int dataSize = data.GetLength(0) * data.GetLength(1) * Marshal.SizeOf(typeof(TDataType));
            byte[] dataBuf = new byte[dataSize];
            Buffer.BlockCopy(data, 0, dataBuf, 0, dataSize);
            return dataBuf;
        }

        internal static byte[] BuildByteData<TDataType>(TDataType[] data)
        {
            int dataSize = data.Length * Marshal.SizeOf(typeof(TDataType));
            byte[] dataBuf = new byte[dataSize];
            Buffer.BlockCopy(data, 0, dataBuf, 0, dataSize);
            return dataBuf;
        }

        internal static void WriteStrToFile(string filePath, string strData, Encoding encode = null)
        {
            StreamWriter writer = null;
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    // TODO to add
                    InitWriteStream(ref writer, filePath, WriteMode.Append, encode);
                    writer.Write(strData);
                }
                else
                {
                    if (null != encode)
                    {
                        System.IO.File.WriteAllText(filePath, strData, encode);
                    }
                    else
                    {
                        System.IO.File.WriteAllText(filePath, strData);
                    }
                }
            }
            catch (ApplicationException ex)
            {
                throw new SeeSharpFileException(SeeSharpFileErrorCode.RuntimeError,
                    i18n.GetFStr("Runtime.WriteFail", ex.Message), ex);
            }
            finally
            {
                ReleaseResource(writer);
            }
        }

        internal static void InitReadStream(ref StreamReader reader, string filePath, Encoding encode)
        {
            reader = (null != encode) ? new StreamReader(filePath, encode) : new StreamReader(filePath);
        }

        internal static void InitWriteStream(ref StreamWriter writer, string filePath, WriteMode writeMode, Encoding encode)
        {
            bool isAppend = (WriteMode.Append == writeMode);
            writer = (null != encode) ? new StreamWriter(filePath, isAppend, encode) : new StreamWriter(filePath, isAppend);
        }

        internal static long InitBinReadStream(ref FileStream stream, ref BinaryReader reader, string filePath)
        {
            stream = new FileStream(filePath, FileMode.Open);
            reader = new BinaryReader(stream);
            return stream.Length;
        }

        public static void InitBinWriteStream(ref FileStream stream, string filePath, WriteMode writeMode)
        {
            FileMode mode = (WriteMode.Append == writeMode) ? FileMode.Append : FileMode.Create;
            stream = new FileStream(filePath, mode);
        }

        internal static void ReleaseResource(IDisposable resource)
        {
            try
            {
                resource?.Dispose();
            }
            catch (ApplicationException ex)
            {
                throw new SeeSharpFileException(SeeSharpFileErrorCode.RuntimeError, ex.Message, ex);
            }
        }

        //通过设置起始位置和行列数读取
        public static TDataType[] StreamReadFromStrFile<TDataType>(StreamReader reader, long index, long startIndex, long size,bool majorOrder, string delims)
        {
            TDataType[] readDatas;
            if (majorOrder == false)
            {
                string lineData = null;//定义一个字符串做每一行的数据
                int skipRowIndex = (int)index;//将开始的行数作为当前读取的行数索引
                do
                {
                    lineData = reader.ReadLine();//在数据流中读取一行
                } while (skipRowIndex-- > 0 && null != lineData);
                // 读取到startRow的下一行，如果为空，则返回null
                if (null == lineData)
                {
                    return null;
                }
                IConvertor convertor = GetConvertor<TDataType>();
                char[] delimArray = delims.ToCharArray();//把一个“，”复制在字符数组
                string[] lineElems = lineData.Split(delimArray);//读到的当前行数据使用“，”分隔后，放在字符串数组中lineElems
                if (size == -1)
                {
                    readDatas = new TDataType[(int)lineElems.Length - startIndex];//定义长度为总列数的数组
                }
                else
                {
                    readDatas = new TDataType[size];//定义长度为总列数的数组
                }


                CopyStrToDst(lineElems, readDatas, convertor, startIndex);//原数据为每行数据，读取到readData中，readdata行索引从0开始,列数组

                return readDatas;
            }
            else
            {
                string lineData = null;//定义一个字符串做每一行的数据
                int skipRowIndex = (int)startIndex;//将开始的行数作为当前读取的行数索引
                do
                {
                    lineData = reader.ReadLine();//在数据流中读取一行
                } while (skipRowIndex-- > 0 && null != lineData);
                // 读取到startRow的下一行，如果为空，则返回null
                if (null == lineData)
                {
                    return null;
                }
                IConvertor convertor = GetConvertor<TDataType>();
                char[] delimArray = delims.ToCharArray();//把一个“，”复制在字符数组
                string[] lineElems = lineData.Split(delimArray);//读到的当前行数据使用“，”分隔后，放在字符串数组中lineElems
                string[] singleColumn = new string[] { lineElems[index] };
                readDatas = new TDataType[size];//定义长度为总列数的数组
                TDataType[] data = new TDataType[1];
                CopyStrToDst(singleColumn, data, convertor);//原数据为每行数据，读取到readData中，readdata行索引从0开始,列数组
                int i = 0;
                readDatas[i++] = data[0];

                while (null != (lineData = reader.ReadLine()) && size > 1)
                {
                    lineElems = lineData.Split(delimArray); //再把该行数据用，分开
                    singleColumn[0] = lineElems[index];
                    CopyStrToDst(singleColumn, data, convertor);//把该行数据写入readdata里
                    readDatas[i++] = data[0];
                    size--;
                }

                return readDatas;
            }
        }

        //通过设置行列数组读取
        public static TDataType[,] StreamReadFromStrFile<TDataType>(StreamReader reader, long[] rowCollection, long[] columnCollection, string delims)
        {
            string lineData = null;//定义一个字符串做每一行的数据
            long rowCount = rowCollection.Length;
            long columnCount = columnCollection.Length;
            int skipRowIndex = (int)rowCollection[0];//将开始的行数作为当前读取的行数索引
            do
            {
                lineData = reader.ReadLine();//在数据流中读取一行
            } while (skipRowIndex-- > 0 && null != lineData);
            // 读取到startRow的下一行，如果为空，则返回null
            if (null == lineData)
            {
                return null;
            }

            IConvertor convertor = GetConvertor<TDataType>();
            char[] delimArray = delims.ToCharArray();//把一个“，”复制在字符数组
            string[] lineElems = lineData.Split(delimArray);//读到的当前行数据使用“，”分隔后，放在字符串数组中lineElems

            if (columnCount == 0)
            {
                columnCollection = new long[lineElems.Length]; //将每一行数据的长度定义为列数
                for (int j = 0; j < columnCount; j++)
                {
                    columnCollection[j] = (uint)j;
                }
            }
            TDataType[,] readDatas = new TDataType[rowCount, columnCount];//定义一个行为行数，列为列数减起始列数的数组
            int i = (int)rowCollection[0];//行索引
            int rowIndex = 0;
            CopyStrToDst(lineElems, readDatas, convertor, rowIndex++, columnCollection);//原数据为每行数据，读取到readData中，readdata行索引从0开始,列数组
            rowCount--; //要读取的总行数减一

            while (null != (lineData = reader.ReadLine()) && rowCount > 0)
            {
                i++;
                if (rowCollection[rowIndex] == i)
                {
                    //读下一行不为null，且要读取的总行数还大于零
                    lineElems = lineData.Split(delimArray); //再把该行数据用，分开
                    CopyStrToDst(lineElems, readDatas, convertor, rowIndex++, columnCollection);//把该行数据写入readdata里
                    rowCount--;

                }
            }

            if (rowCount != 0)
            {
                //如果要读取的行数不为零，也就是读到的该行为null，调整readData的行数
                FitArrayRowCountToData(ref readDatas, (int)(readDatas.GetLength(0) - rowCount));
            }
            return readDatas;
        }
    
        //通过设置起始位置和行列数读取
        public static TDataType[,] StreamReadFromStrFile<TDataType>(StreamReader reader, long startRow, long startColumn, long rowCount, long columnCount, string delims)
        {
            string lineData = null;//定义一个字符串做每一行的数据
            int skipRowIndex = (int)startRow;//将开始的行数作为当前读取的行数索引
            do
            {
                lineData = reader.ReadLine();//在数据流中读取一行
            } while (skipRowIndex-- > 0 && null != lineData);
            // 读取到startRow的下一行，如果为空，则返回null
            if (null == lineData)
            {
                return null;
            }
            IConvertor convertor = GetConvertor<TDataType>();
            char[] delimArray = delims.ToCharArray();//把一个“，”复制在字符数组
            string[] lineElems = lineData.Split(delimArray);//读到的当前行数据使用“，”分隔后，放在字符串数组中lineElems
            if(columnCount==-1)
            {
                 columnCount = (uint)lineElems.Length; //将每一行数据的长度定义为列数
            }

            if (startColumn >= columnCount)
            {
                //如果起始列大于列数，则抛错
                throw new SeeSharpFileException(SeeSharpFileErrorCode.ParamCheckError,
                    i18n.GetStr("ParamCheck.InvalidColIndex"));
            }

            TDataType[,] readDatas = new TDataType[rowCount, columnCount];//定义一个行为行数，列为列数减起始列数的数组
            long rowIndex = 0;//行索引
            long[] columns = new long[columnCount];//列数长度是总列数减去起始列数
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = startColumn++;//列的值从起始列开始增加
            }
            CopyStrToDst(lineElems, readDatas, convertor, rowIndex++, columns);//原数据为每行数据，读取到readData中，readdata行索引从0开始,列数组
            rowCount--; //要读取的总行数减一
            while (null != (lineData = reader.ReadLine()) && rowCount > 0)
            {
                //读下一行不为null，且要读取的总行数还大于零
                lineElems = lineData.Split(delimArray); //再把该行数据用，分开
                CopyStrToDst(lineElems, readDatas, convertor, rowIndex++, columns);//把该行数据写入readdata里
                rowCount--;
            }

            if (rowCount != 0)
            {
                //如果要读取的行数不为零，也就是读到的该行为null，调整readData的行数
                FitArrayRowCountToData(ref readDatas, (int)(readDatas.GetLength(0) - rowCount));
            }
            return readDatas;
        }


        #region Obsolete

        [Obsolete]
        public static TDataType[,] StreamReadFromStrFile<TDataType>(StreamReader reader, uint lineCount, string delims,
      uint startRow, uint startColumn)
        {
            string lineData = null;
            int skipRowIndex = (int)startRow;
            do
            {
                lineData = reader.ReadLine();
            } while (skipRowIndex-- > 0 && null != lineData);
            // 读取到startRow的下一行，如果为空，则返回null
            if (null == lineData)
            {
                return null;
            }
            IConvertor convertor = GetConvertor<TDataType>();
            char[] delimArray = delims.ToCharArray();
            string[] lineElems = lineData.Split(delimArray);
            int colCount = lineElems.Length;
            if (startColumn >= colCount)
            {
                throw new SeeSharpFileException(SeeSharpFileErrorCode.ParamCheckError,
                    i18n.GetStr("ParamCheck.InvalidColIndex"));
            }

            TDataType[,] readDatas = new TDataType[lineCount, colCount - startColumn];
            int rowIndex = 0;
            uint[] columns = new uint[colCount - startColumn];
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = startColumn++;
            }
            CopyStrToDst(lineElems, readDatas, convertor, rowIndex++, columns);
            lineCount--;
            while (null != (lineData = reader.ReadLine()) && lineCount > 0)
            {
                lineElems = lineData.Split(delimArray);
                CopyStrToDst(lineElems, readDatas, convertor, rowIndex++, columns);
                lineCount--;
            }

            if (lineCount != 0)
            {
                FitArrayRowCountToData(ref readDatas, (int)(readDatas.GetLength(0) - lineCount));
            }
            return readDatas;
        }
        [Obsolete]
        public static TDataType[,] StreamReadFromStrFile<TDataType>(StreamReader reader, uint lineCount, string delims,
            uint startRow, uint[] columns)
        {
            string lineData = null;
            int skipRowIndex = (int)startRow;
            do
            {
                lineData = reader.ReadLine();
            } while (skipRowIndex-- > 0 && null != lineData);
            // 读取到startRow的下一行，如果为空，则返回null
            if (null == lineData)
            {
                return null;
            }
            IConvertor convertor = GetConvertor<TDataType>();
            char[] delimArray = delims.ToCharArray();
            string[] lineElems = lineData.Split(delimArray);
            if (columns.Max() >= lineElems.Length)
            {
                throw new SeeSharpFileException(SeeSharpFileErrorCode.ParamCheckError,
                    i18n.GetStr("ParamCheck.InvalidColIndex"));
            }

            TDataType[,] readDatas = new TDataType[lineCount, columns.Length];
            int rowIndex = 0;
            CopyStrToDst(lineElems, readDatas, convertor, rowIndex++, columns);
            lineCount--;
            while (null != (lineData = reader.ReadLine()) && lineCount > 0)
            {
                lineElems = lineData.Split(delimArray);
                CopyStrToDst(lineElems, readDatas, convertor, rowIndex++, columns);
                lineCount--;
            }
            if (lineCount != 0)
            {
                FitArrayRowCountToData(ref readDatas, (int)(readDatas.GetLength(0) - lineCount));
            }

            return readDatas;
        }

        [Obsolete]
        private static void CopyStrToDst<TDataType>(string[] srcStrs, TDataType[,] dstStrs, IConvertor convertor,
    int rowIndex, uint[] columnIndexes)
        {
            int copySize = dstStrs.GetLength(1);
            if (columnIndexes.Length != copySize)
            {
                throw new SeeSharpFileException(SeeSharpFileErrorCode.ParamCheckError,
                    i18n.GetFStr("ParamCheck.ColCountNotFit", rowIndex + 1));
            }
            for (int i = 0; i < copySize; i++)
            {
                dstStrs[rowIndex, i] = (TDataType)convertor.Convert(srcStrs[columnIndexes[i]]);
            }
        }
        #endregion



        private static void FitArrayRowCountToData<TDataType>(ref TDataType[,] readDatas, int rowCount)
        {
            int columnCount = readDatas.GetLength(1);
            TDataType[,] dataAfterFit = new TDataType[rowCount, columnCount];
            // 对于string类型，不能直接拷贝，需要逐个赋值
            if (ReferenceEquals(typeof(string), typeof(TDataType)))
            {
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        dataAfterFit[i, j] = readDatas[i, j];
                    }
                }
            }
            else
            {
                Buffer.BlockCopy(readDatas, 0, dataAfterFit, 0, Marshal.SizeOf(typeof(TDataType)) * rowCount * columnCount);
            }
            readDatas = dataAfterFit;
        }

        private static IConvertor GetConvertor<TDataType>()
        {
            Type dataType = typeof(TDataType);
            if (ReferenceEquals(typeof(double), dataType))
            {
                return new DoubleConvertor();
            }
            else if (ReferenceEquals(typeof(float), dataType))
            {
                return new FloatConvertor();
            }
            else if (ReferenceEquals(typeof(int), dataType))
            {
                return new IntConvertor();
            }
            else if (ReferenceEquals(typeof(uint), dataType))
            {
                return new UIntConvertor();
            }
            else if (ReferenceEquals(typeof(short), dataType))
            {
                return new ShortConvertor();
            }
            else if (ReferenceEquals(typeof(ushort), dataType))
            {
                return new UShortConvertor();
            }
            else if (ReferenceEquals(typeof(string), dataType))
            {
                return new StringConvertor();
            }
            throw new SeeSharpFileException(SeeSharpFileErrorCode.UnsupportedDataType,
                i18n.GetFStr("ParamCheck.UnsupportedType", dataType.Name));
        }

        private static void CopyStrToDst<TDataType>(string[] srcStrs, TDataType[,] dstStrs, IConvertor convertor,
             long rowIndex, long[] columnIndexes)
        {
            int copySize = dstStrs.GetLength(1);
            if (columnIndexes.Length != copySize)
            {
                throw new SeeSharpFileException(SeeSharpFileErrorCode.ParamCheckError,
                    i18n.GetFStr("ParamCheck.ColCountNotFit", rowIndex + 1));
            }
            for (int i = 0; i < copySize; i++)
            {
                dstStrs[rowIndex, i] = (TDataType)convertor.Convert(srcStrs[columnIndexes[i]]);
            }
        }

     

        private static void CopyStrToDst(string[] srcStrs, string[] dstStrs, int offset)
        {
            for (int i = 0; i < srcStrs.Length; i++)
            {
                dstStrs[i] = srcStrs[i + offset];
            }
        }

        public static void StreamWriteStrToFile(StreamWriter writer, IEnumerator enumerator, int rowCount,
            int colCount, string delims)
        {
            StringBuilder strBuffer = new StringBuilder();
            for (int i = 0; i < rowCount; i++)
            {
                strBuffer.Clear();
                for (int j = 0; j < colCount; j++)
                {
                    enumerator.MoveNext();
                    object strElem = enumerator.Current;
                    // TODO 严重影响效率，所以删除，数据的完备性由用户保证
                    //                    if (null == strElem || strElem.ToString().Contains(delims))
                    //                    {
                    //                        throw new Exception($"数据格式错误：数据为空或包含分隔字符串'{delims}'。");
                    //                    }
                    strBuffer.Append(strElem).Append(delims);
                }
                strBuffer.Remove(strBuffer.Length - delims.Length, delims.Length).Append(NewLineDelim);
                writer.Write(strBuffer.ToString());
            }
        }


        internal static uint GetFileLineNum(string filePath)
        {
            FileStream stream = null;
            StreamReader reader = null;
            try
            {
                uint lineCount = 0;
                stream = new FileStream(filePath, FileMode.Open);
                reader = new StreamReader(stream);
                while (null != reader.ReadLine())
                {
                    lineCount++;
                }
                return lineCount;
            }
            finally
            {
                ReleaseResource(reader);
                ReleaseResource(stream);
            }
        }

        private const int BlockSize = 10000000;
        internal static TDataType[] StreamReadFromBinFile<TDataType>(BinaryReader reader, long byteSize, long offset)
        {
            int typeSize = Marshal.SizeOf(typeof(TDataType));
            if (byteSize % (typeSize) != 0)
            {
                throw new SeeSharpFileException(SeeSharpFileErrorCode.ParamCheckError,
                    i18n.GetStr("ParamCheck.InvalidFileSize"));
            }
            TDataType[] dstBuf = new TDataType[(byteSize - offset * typeSize) / typeSize];
            int readSize = BlockSize * typeSize;
            reader.BaseStream.Position = offset * typeSize;
            byte[] readBytes = reader.ReadBytes(readSize); ;
            int dstBufOffset = 0;
            while (0 != readBytes.Length)
            {
                Buffer.BlockCopy(readBytes, 0, dstBuf, dstBufOffset, readBytes.Length);
                dstBufOffset += readBytes.Length;
                readBytes = reader.ReadBytes(readSize);
            }
            return dstBuf;
        }

        internal static TDataType[,] StreamReadFromBinFile<TDataType>(BinaryReader reader, long byteSize,
            int colCount, int offset)
        {
            int typeSize = Marshal.SizeOf(typeof(TDataType));
            if (byteSize % (typeSize * colCount) != 0)
            {
                throw new SeeSharpFileException(SeeSharpFileErrorCode.ParamCheckError,
                    i18n.GetStr("ParamCheck.InvalidFileSize"));
            }
            TDataType[,] dstBuf = new TDataType[(byteSize - offset * typeSize * colCount) / (typeSize * colCount),
                colCount];
            int readSize = BlockSize * typeSize;
            reader.BaseStream.Position = offset * typeSize * colCount;
            byte[] readBytes = reader.ReadBytes(readSize);
            int dstBufOffset = 0;

            while (0 != readBytes.Length)
            {
                Buffer.BlockCopy(readBytes, 0, dstBuf, dstBufOffset, readBytes.Length);
                dstBufOffset += readBytes.Length;
                readBytes = reader.ReadBytes(readSize);
            }
            return dstBuf;
        }

        const int DefaultDstSize = 10000;
        public static string[] StreamReadStrFromBinFile(BinaryReader reader)
        {
            string[] dstData;
            int strCount = ReadStrToArray(reader, out dstData);
            return dstData.Take(strCount).ToArray();
        }

        public static string[,] StreamReadStrFromBinFile(BinaryReader reader, int colNum)
        {
            string[] tmpData;
            int strCount = ReadStrToArray(reader, out tmpData);
            if (strCount % colNum != 0)
            {
                throw new SeeSharpFileException(SeeSharpFileErrorCode.ParamCheckError,
                    i18n.GetStr("ParamCheck.InvalidStrFileSize"));
            }
            string[,] dstData = new string[strCount / colNum, colNum];
            CopyStrToDst(tmpData, dstData);
            return dstData;
        }

        private static void CopyStrToDst<TDataType>(string[] srcStrs, TDataType[] dstStrs,IConvertor convertor, long startIndex = 0)
        {
            if ((srcStrs.Length - startIndex) < dstStrs.Length)
            {
                throw new SeeSharpFileException(SeeSharpFileErrorCode.DataLengthMismatch,
                  i18n.GetStr("DataLengthMismatch"));
            }
            for (int i = (int)startIndex; i < dstStrs.Length + ((int)(startIndex)); i++)
            {
                dstStrs[i - startIndex] = (TDataType)convertor.Convert(srcStrs[i]);
            }
        }

        private static void CopyStrToDst(string[] srcStrs, string[,] dstStrs)
        {
            int index = 0;
            for (int i = 0; i < dstStrs.GetLength(0); i++)
            {
                for (int j = 0; j < dstStrs.GetLength(1); j++)
                {
                    dstStrs[i, j] = srcStrs[index++];
                }
            }
        }

        private static int ReadStrToArray(BinaryReader reader, out string[] dstData)
        {
            int strIndex = 0;
            dstData = new string[DefaultDstSize];
            while (true)
            {
                try
                {
                    while (true)
                    {
                        dstData[strIndex++] = reader.ReadString();
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    string[] tmpBuf = new string[dstData.Length + DefaultDstSize];
                    CopyStrToDst(dstData, tmpBuf, 0);
                    dstData = tmpBuf;
                }
                catch (EndOfStreamException)
                {
                    //igore 该异常时停止
                    break;
                }
            }
            return strIndex;
        }

        public static void StreamwriteDataToFile(FileStream writer, Array data, long byteSize, long offset)
        {
            int writeOffset = 0;
            writer.Position = offset;
            long blockCount = byteSize / BlockSize;
            byte[] tmpBuf = null;
            if (blockCount > 0)
            {
                tmpBuf = new byte[BlockSize];
            }
            while (blockCount-- > 0)
            {
                Buffer.BlockCopy(data, writeOffset, tmpBuf, 0, BlockSize);
                writer.Write(tmpBuf, 0, tmpBuf.Length);
                writeOffset += BlockSize;
            }
            if (0 != byteSize % BlockSize)
            {
                tmpBuf = new byte[byteSize % BlockSize];
                Buffer.BlockCopy(data, writeOffset, tmpBuf, 0, (int)(byteSize % BlockSize));
                writer.Write(tmpBuf, 0, tmpBuf.Length);
            }
        }
    }
}