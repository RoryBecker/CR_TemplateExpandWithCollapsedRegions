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
    public partial class PlugIn1 : StandardPlugIn
    {
        // DXCore-generated code...
        #region InitializePlugIn
        public override void InitializePlugIn()
        {
            base.InitializePlugIn();
            registerTemplateExpandWithCollapsedRegions();
        }
        #endregion
        #region FinalizePlugIn
        public override void FinalizePlugIn()
        {
            //
            // TODO: Add your finalization code here.
            //

            base.FinalizePlugIn();
        }
        #endregion
        public void registerTemplateExpandWithCollapsedRegions()
        {
            DevExpress.CodeRush.Core.Action TemplateExpandWithCollapsedRegions = new DevExpress.CodeRush.Core.Action(components);
            ((System.ComponentModel.ISupportInitialize)(TemplateExpandWithCollapsedRegions)).BeginInit();
            TemplateExpandWithCollapsedRegions.ActionName = "TemplateExpandWithCollapsedRegions";
            TemplateExpandWithCollapsedRegions.ButtonText = "TemplateExpandWithCollapsedRegions"; // Used if button is placed on a menu.
            TemplateExpandWithCollapsedRegions.RegisterInCR = true;
            TemplateExpandWithCollapsedRegions.Execute += TemplateExpandWithCollapsedRegions_Execute;
            TemplateExpandWithCollapsedRegions.CheckAvailability += TemplateExpandWithCollapsedRegions_CheckAvailability;
            ((System.ComponentModel.ISupportInitialize)(TemplateExpandWithCollapsedRegions)).EndInit();
        }
        private void TemplateExpandWithCollapsedRegions_CheckAvailability(CheckActionAvailabilityEventArgs ea)
        {
            DevExpress.CodeRush.Core.Action Action = CodeRush.Actions.Get("TemplateExpand");
            ea.Available = Action == null ? false : Action.GetAvailability("");
        }
        private void TemplateExpandWithCollapsedRegions_Execute(ExecuteEventArgs ea)
        {
            TextDocument ActiveDoc = CodeRush.Documents.ActiveTextDocument;

            using (ActiveDoc.NewCompoundAction("TemplateExpandWithCollapsedRegions"))
            {
                var LinesFromStart = CodeRush.Caret.Line;
                var LinesFromEnd = ActiveDoc.LineCount - LinesFromStart;

                DevExpress.CodeRush.Core.Action Action = CodeRush.Actions.Get("TemplateExpand");
                Action.DoExecute();

                // Calculate line range that template expanded into.
                SourceRange TemplateExpandRange = new SourceRange(new SourcePoint(LinesFromStart, 1),
                                              new SourcePoint(ActiveDoc.LineCount - LinesFromEnd,
                                                  ActiveDoc.GetLine(LinesFromEnd).Length - 1)
                    );
                // Reparse everything just in case.
                CodeRush.Documents.ParseActive();
                CodeRush.Documents.ActiveTextDocument.ParseIfTextChanged();

                // Execute a delayed collapse of all regions in the ExpansionRange
                RangeRegionCollapser Collapser = new RangeRegionCollapser(TemplateExpandRange);
                Collapser.PerformDelayedCollapse(150);

            }
        }
    }
}