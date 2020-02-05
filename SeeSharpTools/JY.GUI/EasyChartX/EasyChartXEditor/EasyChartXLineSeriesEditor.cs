﻿using System;
using System.ComponentModel.Design;
using System.Reflection;

namespace SeeSharpTools.JY.GUI.EasyChartXEditor
{
    internal class EasyChartXLineSeriesEditor : CollectionEditor
    {
        public EasyChartXLineSeriesEditor(Type type) : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(EasyChartXSeries);
        }

        // 开启属性描述
        protected override CollectionForm CreateCollectionForm()
        {
            CollectionForm frm = base.CreateCollectionForm();
            FieldInfo fileinfo = frm.GetType()
                .GetField("propertyBrowser", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fileinfo != null)
            {
                (fileinfo.GetValue(frm) as System.Windows.Forms.PropertyGrid).HelpVisible = true;
            }
             return frm;
        }
    }
}