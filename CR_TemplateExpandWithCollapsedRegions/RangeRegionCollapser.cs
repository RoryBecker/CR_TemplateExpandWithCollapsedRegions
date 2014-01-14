using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.PlugInCore;
using DevExpress.CodeRush.StructuralParser;

namespace CR_TemplateExpandWithCollapsedRegions
{
    public class RangeRegionCollapser
    {

        private readonly SourceRange _range;
        private DelayedRunner _runner = new DelayedRunner();
        public RangeRegionCollapser(SourceRange range)
        {
            _range = range;
        }
        public void PerformDelayedCollapse(int delay)
        {
            _runner.RunFuncUntilSuccess(CollapseRegions, delay, 10000);
        }
        private bool CollapseRegions()
        {
            TextDocument ActiveDoc = CodeRush.Documents.ActiveTextDocument;
            bool Worked = true;
            var RegionsInRange = from item in ActiveDoc.Regions.ToArray()
                                 where ((RegionDirective)item).Range.IntersectsWith(_range)
                                 select (RegionDirective)item;
            if (RegionsInRange.Count() == 0)
            {
                return true; // nothing to check
            }
            foreach (RegionDirective region in RegionsInRange)
            {
                region.CollapseInView(ActiveDoc.ActiveView);
                if (!region.Collapsed)
                    Worked = false;
            }
            return Worked;
        }
    }
}
