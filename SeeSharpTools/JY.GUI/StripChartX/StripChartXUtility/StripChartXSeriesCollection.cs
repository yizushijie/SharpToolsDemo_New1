﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using SeeSharpTools.JY.GUI.StripChartXUtility;

namespace SeeSharpTools.JY.GUI
{
    public class StripChartXSeriesCollection : IList<StripChartXSeries>
    {
        private readonly List<StripChartXSeries> _lineSeries;

        private readonly Color[] _seriesPalette = new Color[]
        {
            Color.Red, Color.Blue, Color.DeepPink, Color.Navy, Color.DarkGreen, Color.OrangeRed, Color.DarkCyan,
            Color.Black
        };

        private readonly SeriesCollection _plotSeries;
        private readonly StripChartX _parentChart;

        internal StripChartXSeriesCollection(SeriesCollection chartSeries, StripChartX parentChart)
        {
            this._plotSeries = chartSeries;
            _lineSeries = new List<StripChartXSeries>(Constants.DefaultMaxSeriesCount);
            this._parentChart = parentChart;
            // 默认添加n个Series
            foreach (Series plotSeries in _plotSeries)
            {
                StripChartXSeries series = new StripChartXSeries();
                series.SetSeriesCollecton(this);
                series.Name = plotSeries.Name;
                series.AdaptBaseSeries(plotSeries);
                // 为了判断是否为设计时，只能使用原集合的Add
                _lineSeries.Add(series);
            }
        }

        private const string SeriesNameFormat = "Series{0}";

        internal void ReAdaptSeriesFromEnd(int endIndex = 0)
        {
            for (int i = _lineSeries.Count - 1; i >= endIndex; i--)
            {
                if (_plotSeries.Count > i)
                {
                    _lineSeries[i].AdaptBaseSeries(_plotSeries[i]);
                }
            }
        }

        internal void ReAdaptSeriesFromFront(int startIndex = 0)
        {
            for (int i = startIndex; i < _lineSeries.Count; i++)
            {
                _lineSeries[i].AdaptBaseSeries(_plotSeries[i]);
            }
        }

        public void AdaptSeriesCount(int plotSeriesCount)
        {
            while (_lineSeries.Count < plotSeriesCount)
            {
                AddInternal(new StripChartXSeries());
            }
            for (int i = plotSeriesCount; i < _lineSeries.Count; i++)
            {
                _lineSeries[i].DetachBaseSeries();
            }
        }


        IEnumerator<StripChartXSeries> IEnumerable<StripChartXSeries>.GetEnumerator()
        {
            return _lineSeries.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _lineSeries.GetEnumerator();
        }

        public void CopyTo(StripChartXSeries[] array, int index)
        {
            return;
        }

        public int Count => _lineSeries.Count;
        public object SyncRoot { get; }
        public bool IsSynchronized { get; }

        private bool _designEndFlag = false;
        // 用于外部的Add方法，主要是设计时会使用
        public void Add(StripChartXSeries item)
        {
            if (null == item || _lineSeries.Count >= _parentChart.Miscellaneous.MaxSeriesCount)
            {
                return;
            }
            if (!_designEndFlag)
            {
                _lineSeries.Clear();
//                _designStartFlag = true;
            }
            AddInternal(item);
        }

        // 内部的Add方法，不会调用清空列表的部分
        internal void AddInternal(StripChartXSeries item)
        {
            // 开始内部Add后即设计时代码已结束
            _designEndFlag = true;
            item.SetSeriesCollecton(this);
            int index = _lineSeries.FindIndex(existItem => existItem.Name.Equals(item.Name));
            if (index >= 0 && index < _lineSeries.Count)
            {
                _lineSeries[index] = item;
                if (_plotSeries.Count > index)
                {
                    item.AdaptBaseSeries(_plotSeries[index]);
                }
            }
            else
            {
                if (null == item.Name || "".Equals(item.Name))
                {
                    string seriesName = "";
                    int seriesIndex = _lineSeries.Count + 1;
                    do
                    {
                        seriesName = string.Format(SeriesNameFormat, seriesIndex++);
                    } while (_lineSeries.Any(existItem => existItem.Name.Equals(seriesName)));
                    item.Name = seriesName;
                    item.Color = _seriesPalette[_lineSeries.Count%_seriesPalette.Length];
                }
                _lineSeries.Add(item);
                if (_plotSeries.Count >= _lineSeries.Count)
                {
                    item.AdaptBaseSeries(_plotSeries[_lineSeries.Count - 1]);
                }
            }
        }

//        public bool Contains(object value)
        public bool Contains(StripChartXSeries item)
        {
            if (null == item)
            {
                return false;
            }
            return _lineSeries.Contains(item);
        }

        public void Clear()
        {
//            if (_plotSeries.Count > 0)
//            {
//                return;
//            }
            _lineSeries.Clear();
        }

        public int IndexOf(StripChartXSeries item)
        {
            if (null == item)
            {
                return -1;
            }
            return _lineSeries.IndexOf(item);
        }

        public void Insert(int index, StripChartXSeries item)
        {
            if (null == item || index >= _lineSeries.Count || _lineSeries.Count >= _parentChart.Miscellaneous.MaxSeriesCount
                || _lineSeries.Any(existItem => existItem.Name.Equals(item.Name)))
            {
                return;
            }
            item.SetSeriesCollecton(this);
            if (null == item.Name || "".Equals(item.Name))
            {
                string seriesName = "";
                int seriesIndex = _lineSeries.Count + 1;
                do
                {
                    seriesName = string.Format(SeriesNameFormat, seriesIndex++);
                } while (!_lineSeries.Any(existItem => existItem.Name.Equals(seriesName)));
                item.Name = seriesName;
            }
            _lineSeries.Insert(index, item);
            ReAdaptSeriesFromFront(index);
        }

//        public void Remove(object value)
        public bool Remove(StripChartXSeries item)
        {
            int index = _lineSeries.IndexOf(item);
            // LineSeries个数不能少于PlotSeries的个数
            if (_plotSeries.Count > _lineSeries.Count - 1 || null == item || index < 0)
            {
                return false;
            }
            _lineSeries.Remove(item);
            ReAdaptSeriesFromEnd(index);
            return true;
        }

        //        public void Remove(object value)
        public bool Remove(object value)
        {
            StripChartXSeries item = value as StripChartXSeries;
            int index = _lineSeries.IndexOf(item);
            // LineSeries个数不能少于PlotSeries的个数
            if (_plotSeries.Count > _lineSeries.Count - 1 || null == item || index < 0)
            {
                return false;
            }
            _lineSeries.Remove(item);
            ReAdaptSeriesFromEnd(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            // LineSeries个数不能少于PlotSeries的个数
            if (_plotSeries.Count > _lineSeries.Count - 1)
            {
                return;
            }
            _lineSeries.RemoveAt(index);
            ReAdaptSeriesFromEnd(index);
        }

//        public object this[int index]
        public StripChartXSeries this[int index]
        {
            get { return _lineSeries[index]; }
            set
            {
                if (null == value)
                {
                    return;
                }
                if (_plotSeries.Count > index)
                {
                    value.AdaptBaseSeries(_plotSeries[index]);
                }
                _lineSeries[index] = value;
            }
        }

        internal void RefreshPlotAxis(StripChartXSeries series)
        {
            bool isSplitView = _parentChart.SplitView;
            if (null == series)
            {
                foreach (StripChartXSeries xSeries in _lineSeries)
                {
                    xSeries.RefreshPlotAxis(isSplitView);
                }
            }
            else
            {
                series.RefreshPlotAxis(isSplitView);
            }
        }

        public bool IsReadOnly => false;
        public bool IsFixedSize => false;
        
    }
}