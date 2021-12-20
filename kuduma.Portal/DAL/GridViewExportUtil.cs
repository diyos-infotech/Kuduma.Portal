using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using OfficeOpenXml;
/// <summary>
/// 
/// </summary>
public class GridViewExportUtil
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="gv"></param>
    /// 
    public void NewExportExcel(string fileName, DataTable dt)
    {

        var products = dt;
        ExcelPackage excel = new ExcelPackage();
        var workSheet = excel.Workbook.Worksheets.Add(fileName);
        var totalCols = products.Columns.Count;
        var totalRows = products.Rows.Count;

        for (var col = 1; col <= totalCols; col++)
        {
            workSheet.Cells[1, col].Value = products.Columns[col - 1].ColumnName;
            workSheet.Cells[1, col].Style.Font.Bold = true;

        }
        for (var row = 1; row <= totalRows; row++)
        {
            for (var col = 0; col < totalCols; col++)
            {
                workSheet.Cells[row + 1, col + 1].Value = products.Rows[row - 1][col];

            }
        }
        using (var memoryStream = new MemoryStream())
        {
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment ;filename=\"" + fileName + "\"");
            excel.SaveAs(memoryStream);
            memoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }
    public void Export(string fileName, GridView gv)
    {
        GridViewExportUtil gve = new GridViewExportUtil();
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader(
            "content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/ms-excel";

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                //  Create a form to contain the grid
                Table table = new Table();
                table.BorderStyle = BorderStyle.Solid;
                table.GridLines = GridLines.Both;
                //  add the header row to the table
                if (gv.HeaderRow != null)
                {
                    gve.PrepareControlForExport(gv.HeaderRow);
                    table.Rows.Add(gv.HeaderRow);
                }

                //  add each of the data rows to the table
                foreach (GridViewRow row in gv.Rows)
                {
                    gve.PrepareControlForExport(row);
                    table.Rows.Add(row);
                }

                //  add the footer row to the table
                if (gv.FooterRow != null)
                {
                    gve.PrepareControlForExport(gv.FooterRow);
                    table.Rows.Add(gv.FooterRow);
                }

                //  render the table into the htmlwriter
                table.RenderControl(htw);

                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();
            }
        }
    }

    /// <summary>
    /// Replace any of the contained controls with literals
    /// </summary>
    /// <param name="control"></param>
    private void PrepareControlForExport(Control control)
    {
        GridViewExportUtil gve = new GridViewExportUtil();
        for (int i = 0; i < control.Controls.Count; i++)
        {
            Control current = control.Controls[i];
            if (current is LinkButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
            }
            else if (current is ImageButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
            }
            else if (current is HyperLink)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
            }
            else if (current is DropDownList)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
            }
            else if (current is CheckBox)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
            }

            if (current.HasControls())
            {
                gve.PrepareControlForExport(current);
            }
        }
    }

    public void ExportGrid(string fileName, HiddenField hidGridView)
    {
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.Charset = "";
        System.IO.StringWriter stringwriter = new System.IO.StringWriter();
        stringwriter.Write(System.Web.HttpUtility.HtmlDecode(hidGridView.Value));
        HttpContext.Current.Response.Write(style);
        HttpContext.Current.Response.Write(stringwriter.ToString());
        HttpContext.Current.Response.End();
    }

    public void ExporttoExcel1(DataTable table, string line, string line1, string line2)
    {


        string filename = line2 + ".xls";
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename='" + line2 + "'.xls");

        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = table.Columns.Count;

        HttpContext.Current.Response.Write("<TR valign='top'>");

        HttpContext.Current.Response.Write("<Td align='center' colspan= " + columnscount + ">");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line);
        HttpContext.Current.Response.Write("</B>");

        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");


        HttpContext.Current.Response.Write("<TR valign='top'>");

        HttpContext.Current.Response.Write("<Td align='left' colspan= " + columnscount + ">");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line1);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("</TR>");


        for (int j = 0; j < columnscount; j++)
        {
            //write in new column

            HttpContext.Current.Response.Write("<Td valign='middle'>");

            //Get column headers  and make it as bold in excel columns
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.Columns[j].ToString());

            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
        }

        HttpContext.Current.Response.Write("</TR>");

        foreach (DataRow row in table.Rows)
        {//write in new row
            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row[i].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write(style);
            }

            HttpContext.Current.Response.Write("</TR>");
        }
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public void NewExport(string fileName, GridView gv)
    {


        DataTable dt = new DataTable();

        // add the columns to the datatable            
        if (gv.HeaderRow != null)
        {

            for (int i = 0; i < gv.HeaderRow.Cells.Count; i++)
            {
                dt.Columns.Add(gv.HeaderRow.Cells[i].Text);
            }

        }

        //  add each of the data rows to the table
        foreach (GridViewRow row in gv.Rows)
        {
            DataRow dr;
            dr = dt.NewRow();

            for (int i = 0; i < row.Cells.Count; i++)
            {
                dr[i] = row.Cells[i].Text.Replace(" ", "");
            }
            dt.Rows.Add(dr);
        }



        //  add the footer row to the table
        if (gv.FooterRow != null)
        {
            DataRow dr;
            dr = dt.NewRow();

            for (int i = 0; i < gv.FooterRow.Cells.Count; i++)
            {
                dr[i] = gv.FooterRow.Cells[i].Text.Replace("&nbsp;", "");
            }


            dt.Rows.Add(dr);
        }

        var products = dt;
        ExcelPackage excel = new ExcelPackage();
        var workSheet = excel.Workbook.Worksheets.Add(fileName);
        var totalCols = products.Columns.Count;
        var totalRows = products.Rows.Count;

        for (var col = 1; col <= totalCols; col++)
        {
            workSheet.Cells[1, col].Value = products.Columns[col - 1].ColumnName;
            workSheet.Cells[1, col].Style.Font.Bold = true;

        }
        for (var row = 1; row <= totalRows; row++)
        {
            for (var col = 0; col < totalCols; col++)
            {
                workSheet.Cells[row + 1, col + 1].Value = products.Rows[row - 1][col];
            }
        }


        using (var memoryStream = new MemoryStream())
        {
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment ;filename=\"" + fileName + "\"");
            excel.SaveAs(memoryStream);
            memoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

    }

    public void ExporttoExcelNewPaysheet(DataTable table, string line, string line2, string filename)
    {
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");

        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = table.Columns.Count;

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='left' colspan= '" + columnscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='Left' colspan= '" + columnscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line2);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        columnscount = 24;
        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='center' colspan= '" + columnscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        columnscount = 17;
        HttpContext.Current.Response.Write("<Td align='center' colspan= '" + columnscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("Presents Salaries");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        columnscount = 2;
        HttpContext.Current.Response.Write("<Td align='center' colspan= '" + columnscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("Employer Contribution");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        columnscount = 2;
        HttpContext.Current.Response.Write("<Td align='center' colspan= '" + columnscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("Employee Contribution");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        columnscount = 9;
        HttpContext.Current.Response.Write("<Td align='left' colspan= '" + columnscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("</TR>");

        columnscount = table.Columns.Count;
        for (int j = 0; j < columnscount; j++)
        {
            //write in new column

            HttpContext.Current.Response.Write("<Td valign='middle'>");

            //Get column headers  and make it as bold in excel columns
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.Columns[j].ToString());

            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
        }

        HttpContext.Current.Response.Write("</TR>");

        foreach (DataRow row in table.Rows)
        {//write in new row
            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row[i].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write(style);
            }

            HttpContext.Current.Response.Write("</TR>");
        }
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public void ExportGridNew(HiddenField hidGridView, int count, string line, string line2)
    {

        string filename = line2 + ".xls";
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename=\"" + filename + "\""));
        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        HttpContext.Current.Response.Write("<TR valign='top'>");

        HttpContext.Current.Response.Write("<Td style='border:none' align='center' colspan='" + count + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("FORM-XVII REGISTER OF WAGES (VIDE RULE 78(1)(a)(i) OF CONTRACT LABOUR (REG. & ABOLITION)CENTRAL & A.P.RULES");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR  valign='top'>");
        HttpContext.Current.Response.Write("<Td style='border:none' align='center' colspan='" + count + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");
        HttpContext.Current.Response.Write("</TR>");

        // for (int j = 0; j < columnscount; j++)
        //{
        //    //write in new column

        //    HttpContext.Current.Response.Write("<Td valign='middle'>");

        //    //Get column headers  and make it as bold in excel columns
        //    HttpContext.Current.Response.Write("<B>");
        //    HttpContext.Current.Response.Write(table.Columns[j].ToString());

        //    HttpContext.Current.Response.Write("</B>");
        //    HttpContext.Current.Response.Write("</Td>");
        //}

        //HttpContext.Current.Response.Write("</TR>");

        //foreach (DataRow row in table.Rows)
        //{//write in new row
        //    HttpContext.Current.Response.Write("<TR>");
        //    for (int i = 0; i < table.Columns.Count; i++)
        //    {
        //        HttpContext.Current.Response.Write("<Td>");
        //        HttpContext.Current.Response.Write(row[i].ToString());
        //        HttpContext.Current.Response.Write("</Td>");
        //        HttpContext.Current.Response.Write(style);
        //    }

        //    HttpContext.Current.Response.Write("</TR>");
        //}
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        System.IO.StringWriter stringwriter = new System.IO.StringWriter();
        stringwriter.Write(System.Web.HttpUtility.HtmlDecode(hidGridView.Value));
        HttpContext.Current.Response.Write(style);
        HttpContext.Current.Response.Write(stringwriter.ToString());
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
        HttpContext.Current.Response.End();
    }

    public void ExporttoExcelXIII(DataTable table, string FileName, string line1, string line2, string line3, string line4)
    {


        // filename = "SalarySheet.xls";
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename='" + FileName + "'.xls");

        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = table.Columns.Count;

        //33

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='center' colspan= '" + columnscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line1);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='center' colspan= '" + columnscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line2);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='Left' colspan= '" + columnscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line3);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='Left' colspan= '" + columnscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line4);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");


        for (int j = 0; j < columnscount; j++)
        {
            //write in new column

            HttpContext.Current.Response.Write("<Td valign='middle'>");

            //Get column headers  and make it as bold in excel columns
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.Columns[j].ToString());

            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
        }

        HttpContext.Current.Response.Write("</TR>");

        foreach (DataRow row in table.Rows)
        {//write in new row
            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row[i].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write(style);
            }

            HttpContext.Current.Response.Write("</TR>");
        }
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public void ExporttoExcelsalaryded(DataTable table, string FileName, string line1, string line2, string line3, string line4, string line5, string line6)
    {


        // filename = "SalarySheet.xls";
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename='" + FileName + "'.xls");

        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = table.Columns.Count;

        //33

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='left' colspan= 11>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line2);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td align='Left' colspan= 11>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line3);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td align='Left' colspan= 11>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line4);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<Td align='center' colspan= 33>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line6);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("</TR>");

        line6 = "RATE OF WAGES/SALARY";
        HttpContext.Current.Response.Write("</TR>");
        HttpContext.Current.Response.Write("<Td align='center'colspan= 33>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line6);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");


        HttpContext.Current.Response.Write("</TR>");


        for (int j = 0; j < columnscount; j++)
        {
            //write in new column

            HttpContext.Current.Response.Write("<Td valign='middle'>");

            //Get column headers  and make it as bold in excel columns
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.Columns[j].ToString());

            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
        }

        HttpContext.Current.Response.Write("</TR>");

        foreach (DataRow row in table.Rows)
        {//write in new row
            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row[i].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write(style);
            }

            HttpContext.Current.Response.Write("</TR>");
        }
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public void ExporttoExcelnew(DataTable table)
    {


        string filename = "Formc.xls";
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=FormC.xls");

        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = table.Columns.Count;




        for (int j = 0; j < columnscount; j++)
        {
            //write in new column

            HttpContext.Current.Response.Write("<Td valign='middle'>");

            //Get column headers  and make it as bold in excel columns
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.Columns[j].ToString());

            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
        }

        HttpContext.Current.Response.Write("</TR>");

        foreach (DataRow row in table.Rows)
        {//write in new row
            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row[i].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write(style);
            }

            HttpContext.Current.Response.Write("</TR>");
        }
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public void ExporttoExcelLWfmaster(DataTable table, string fileName)
    {


        string filename = "Formc.xls";
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=LWFMaster.xls");

        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = table.Columns.Count;




        for (int j = 0; j < columnscount; j++)
        {
            //write in new column

            HttpContext.Current.Response.Write("<Td valign='middle'>");

            //Get column headers  and make it as bold in excel columns
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.Columns[j].ToString());

            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
        }

        HttpContext.Current.Response.Write("</TR>");

        foreach (DataRow row in table.Rows)
        {//write in new row
            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row[i].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write(style);
            }

            HttpContext.Current.Response.Write("</TR>");
        }
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public void ExportGridForWagesheetReport(string fileName, int countduties, int countfixedwages, int countearnings, int countdedutions, int countpfempr, int countAdvBonus, int countnetpay, int Empdetailscount, string Form, string wages, string Rule, string ContractorName, string WorkLocation, int count, HiddenField hidGridView)
    {
        string filename = fileName;
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", filename));

        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='0' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = 30;

        //HttpContext.Current.Response.Write("<TR valign='top'>");

        //HttpContext.Current.Response.Write("<Td style='border:0; align='center'  colspan=10 >");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write("");
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");

        //HttpContext.Current.Response.Write("<TR valign='top'>");

        //HttpContext.Current.Response.Write("<Td border='0'; align='center'  colspan= 20>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write(Form);
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");
        //HttpContext.Current.Response.Write("</TR>");

        //HttpContext.Current.Response.Write("<TR valign='top'>");

        //HttpContext.Current.Response.Write("<Td border='0'; align='center'  colspan= 20>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write(wages);
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");
        //HttpContext.Current.Response.Write("</TR>");

        //HttpContext.Current.Response.Write("<TR valign='top'>");
        //HttpContext.Current.Response.Write("<Td border='0'; align='center' colspan= 20>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write(Rule);
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");
        //HttpContext.Current.Response.Write("</TR>");

        //HttpContext.Current.Response.Write("<TR valign='top'>");
        //HttpContext.Current.Response.Write("<Td border='0'; align='left' colspan= '" + count + "'>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write(ContractorName);
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");

        ////HttpContext.Current.Response.Write("<Td style='border:0; align='right'  colspan= '" + count/2 + "'>");
        ////HttpContext.Current.Response.Write("<B>");
        ////HttpContext.Current.Response.Write(Address);
        ////HttpContext.Current.Response.Write("</B>");
        ////HttpContext.Current.Response.Write("</Td>");
        //HttpContext.Current.Response.Write("</TR>");

        //HttpContext.Current.Response.Write("<TR valign='top'>");
        //HttpContext.Current.Response.Write("<Td border='0'; align='left'  colspan= '" + count + "'>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write(WorkLocation);
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");

        //HttpContext.Current.Response.Write("<Td style='border:0; align='right'  colspan= '" + count / 2 + "'>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write(PrincipalEmployeer);
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");
        //HttpContext.Current.Response.Write("</TR>");

        //HttpContext.Current.Response.Write("<TR valign='top'>");
        //HttpContext.Current.Response.Write("<Td style='border:0; align='left'  colspan= '" + count / 2 + "'>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write("");
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");

        //HttpContext.Current.Response.Write("<Td style='border:0; align='right'  colspan= '" + count / 2 + "'>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write(Wageperiod);
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");
        //HttpContext.Current.Response.Write("</TR>");


        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
         "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
         "style='font-size:11.0pt; font-family:calibri; background:white;'>");
        HttpContext.Current.Response.Write("<TR valign='top'>");

        HttpContext.Current.Response.Write("<Td border='1'; align='center'  colspan= '" + Empdetailscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("EMPLOYEES DETAILS");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        //HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<Td border='1' align='center'  colspan='" + countfixedwages + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("FIXED WAGES");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td border='1' align='center'  colspan='" + countduties + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("WORKED");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");


        HttpContext.Current.Response.Write("<Td border='1' align='center'  colspan='" + countearnings + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("AMOUNT OF WAGES EARNED");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");


        HttpContext.Current.Response.Write("<Td border='1'; align='center'  colspan='" + countdedutions + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("DEDUCTIONS");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        //HttpContext.Current.Response.Write("<Td border='1'; align='center'  colspan=1>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write(" ");
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");

        //HttpContext.Current.Response.Write("<Td border='1'; align='center'  colspan='" + countpfempr + "'>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write(" ");
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");

        if (countAdvBonus > 0)
        {
            HttpContext.Current.Response.Write("<Td border='1'; align='center'  colspan='" + countAdvBonus + "'>");
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write("");
            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
        }

        HttpContext.Current.Response.Write("<Td border='1'; align='center'  colspan='" + countnetpay + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("NET PAID");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td border='1'; align='center'  colspan= 1>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(" ");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("</TR>");
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");


        System.IO.StringWriter stringwriter = new System.IO.StringWriter();
        stringwriter.Write(System.Web.HttpUtility.HtmlDecode(hidGridView.Value));
        HttpContext.Current.Response.Write(style);
        HttpContext.Current.Response.Write(stringwriter.ToString());
        HttpContext.Current.Response.End();


    }


    public void ExportGridForWagesheetctcReport(string fileName, int countduties, int countfixedwages, int countearnings, int countdedutions, int countpfempr, int countAdvBonus, int countnetpay, int Empdetailscount, string Form, string wages, string Rule, string ContractorName, string WorkLocation, int count,int othrsount,string line2, HiddenField hidGridView)
    {
        string filename = fileName;
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", filename));

        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='0' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = 30;

        HttpContext.Current.Response.Write("<TR valign='top'>");

        HttpContext.Current.Response.Write("<Td style='border:0; align='center'  colspan=25>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(ContractorName);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR valign='top'>");

        HttpContext.Current.Response.Write("<Td style='border:0; align='center'  colspan=25 >");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line2);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");
        //HttpContext.Current.Response.Write("<TR valign='top'>");

        //HttpContext.Current.Response.Write("<Td border='0'; align='center'  colspan= 20>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write(Form);
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");
        //HttpContext.Current.Response.Write("</TR>");

        //HttpContext.Current.Response.Write("<TR valign='top'>");

        //HttpContext.Current.Response.Write("<Td border='0'; align='center'  colspan= 20>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write(wages);
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");
        //HttpContext.Current.Response.Write("</TR>");

        //HttpContext.Current.Response.Write("<TR valign='top'>");
        //HttpContext.Current.Response.Write("<Td border='0'; align='center' colspan= 20>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write(Rule);
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");
        //HttpContext.Current.Response.Write("</TR>");

        //HttpContext.Current.Response.Write("<TR valign='top'>");
        //HttpContext.Current.Response.Write("<Td border='0'; align='left' colspan= '" + count + "'>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write(ContractorName);
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");

        ////HttpContext.Current.Response.Write("<Td style='border:0; align='right'  colspan= '" + count/2 + "'>");
        ////HttpContext.Current.Response.Write("<B>");
        ////HttpContext.Current.Response.Write(Address);
        ////HttpContext.Current.Response.Write("</B>");
        ////HttpContext.Current.Response.Write("</Td>");
        //HttpContext.Current.Response.Write("</TR>");

        //HttpContext.Current.Response.Write("<TR valign='top'>");
        //HttpContext.Current.Response.Write("<Td border='0'; align='left'  colspan= '" + count + "'>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write(WorkLocation);
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");

        //HttpContext.Current.Response.Write("<Td style='border:0; align='right'  colspan= '" + count / 2 + "'>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write(PrincipalEmployeer);
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");
        //HttpContext.Current.Response.Write("</TR>");

        //HttpContext.Current.Response.Write("<TR valign='top'>");
        //HttpContext.Current.Response.Write("<Td style='border:0; align='left'  colspan= '" + count / 2 + "'>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write("");
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");

        //HttpContext.Current.Response.Write("<Td style='border:0; align='right'  colspan= '" + count / 2 + "'>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write(Wageperiod);
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");
        //HttpContext.Current.Response.Write("</TR>");


        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
         "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
         "style='font-size:11.0pt; font-family:calibri; background:white;'>");
        HttpContext.Current.Response.Write("<TR valign='top'>");

        HttpContext.Current.Response.Write("<Td border='1'; align='center'  colspan= '" + Empdetailscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        //HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<Td border='1' align='center'  colspan='" + countduties + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td border='1' align='center'  colspan='" + othrsount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");


        HttpContext.Current.Response.Write("<Td border='1' align='center'  colspan='" + countfixedwages + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("Total Gross Wages");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

      


        HttpContext.Current.Response.Write("<Td border='1' align='center'  colspan='" + countearnings + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("Earned Wages");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");


        HttpContext.Current.Response.Write("<Td border='1'; align='center'  colspan='" + countdedutions + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("Employee Deductions");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        //HttpContext.Current.Response.Write("<Td border='1'; align='center'  colspan=1>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write(" ");
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td border='1'; align='center'  colspan='" + countpfempr + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(" ");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        //if (countAdvBonus > 0)
        //{
        //    HttpContext.Current.Response.Write("<Td border='1'; align='center'  colspan='" + countAdvBonus + "'>");
        //    HttpContext.Current.Response.Write("<B>");
        //    HttpContext.Current.Response.Write("");
        //    HttpContext.Current.Response.Write("</B>");
        //    HttpContext.Current.Response.Write("</Td>");
        //}

        //HttpContext.Current.Response.Write("<Td border='1'; align='center'  colspan='" + countnetpay + "'>");
        //HttpContext.Current.Response.Write("<B>");
        //HttpContext.Current.Response.Write("NET PAID");
        //HttpContext.Current.Response.Write("</B>");
        //HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td border='1'; align='center'  colspan= 1>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(" ");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("</TR>");
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");


        System.IO.StringWriter stringwriter = new System.IO.StringWriter();
        stringwriter.Write(System.Web.HttpUtility.HtmlDecode(hidGridView.Value));
        HttpContext.Current.Response.Write(style);
        HttpContext.Current.Response.Write(stringwriter.ToString());
        HttpContext.Current.Response.End();


    }

    public void ExporttoExcelded(DataTable table, string FileName, string line1, string line2, string line3, string line4, string line5, string line6)
    {


        // filename = "SalarySheet.xls";
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename='" + FileName + "'.xls");

        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = table.Columns.Count;

        //24

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='left' colspan= 8>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line2);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td align='Left' colspan= 8>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line3);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td align='Left' colspan= 8>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line4);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<Td align='center' colspan= 24>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line6);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("</TR>");

        line6 = "DEDUCTIONS";
        HttpContext.Current.Response.Write("</TR>");
        HttpContext.Current.Response.Write("<Td align='center'colspan= 24>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line6);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");


        HttpContext.Current.Response.Write("</TR>");


        for (int j = 0; j < columnscount; j++)
        {
            //write in new column

            HttpContext.Current.Response.Write("<Td valign='middle'>");

            //Get column headers  and make it as bold in excel columns
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.Columns[j].ToString());

            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
        }

        HttpContext.Current.Response.Write("</TR>");

        foreach (DataRow row in table.Rows)
        {//write in new row
            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row[i].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write(style);
            }

            HttpContext.Current.Response.Write("</TR>");
        }
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public void ExporttoExcelNew(DataTable table, string FileName, string line, string line1, string line2, string line3, string line4, string line5, string line6)
    {


        // filename = "SalarySheet.xls";
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename='" + FileName + "'.xls");

        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = table.Columns.Count;


        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='center'colspan= 49>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='left' colspan= 13>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line1);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td align='Left' colspan= 12>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line2);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td align='Left' colspan= 12>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line3);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td align='Left' colspan= 12>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line4);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("</TR>");


        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='left' colspan= 10>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line5);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td align='center'colspan= 31>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line6);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td align='Left' colspan= 8>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line5);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("</TR>");


        for (int j = 0; j < columnscount; j++)
        {
            //write in new column

            HttpContext.Current.Response.Write("<Td valign='middle'>");

            //Get column headers  and make it as bold in excel columns
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.Columns[j].ToString());

            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
        }

        HttpContext.Current.Response.Write("</TR>");

        foreach (DataRow row in table.Rows)
        {//write in new row
            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<Td class='text'>");
                HttpContext.Current.Response.Write(row[i].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write(style);
            }

            HttpContext.Current.Response.Write("</TR>");
        }
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public void ExporttoExcelAttendance(DataTable table, string FileName, string line1, string line2, string line3, string line4, string line5, string line6)
    {


        // filename = "SalarySheet.xls";
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename='" + FileName + "'.xls");

        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = table.Columns.Count;

        //40

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='left' colspan= 14>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line2);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td align='Left' colspan= 14>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line3);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td align='Left' colspan= 13>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line4);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='left' colspan= 20>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line5);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td align='Left' colspan= 21>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line6);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");



        HttpContext.Current.Response.Write("</TR>");


        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='left' colspan= 9>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line1);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        line6 = "ATTENDANCE";
        HttpContext.Current.Response.Write("<Td align='center'colspan= 31>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line6);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td align='Left' colspan= 1>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line1);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("</TR>");


        for (int j = 0; j < columnscount; j++)
        {
            //write in new column

            HttpContext.Current.Response.Write("<Td valign='middle'>");

            //Get column headers  and make it as bold in excel columns
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.Columns[j].ToString());

            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
        }

        HttpContext.Current.Response.Write("</TR>");

        foreach (DataRow row in table.Rows)
        {//write in new row
            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<Td class='text'>");
                HttpContext.Current.Response.Write(row[i].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write(style);
            }

            HttpContext.Current.Response.Write("</TR>");
        }
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public void ExporttoExceFromTpaysheet(DataTable table, string FileName, string line, string line1, string line2, string line3, string line4, string line5, string line6)
    {


        // filename = "SalarySheet.xls";
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename='" + FileName + "'.xls");

        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = table.Columns.Count;


        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='center' style='border:none;' colspan= 57>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        //Row2

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='left' style='border:none;' colspan= 15>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line1);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("<Td align='Left' style='border:none;' colspan= 14>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line2);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("<Td align='Left' style='border:none;' colspan= 14>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line3);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("<Td align='Left' style='border:none;' colspan= 14>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line4);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        //Row3

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='left' colspan= 6>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line5);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        line6 = "EARNED WAGES & OTHER ALLOWANCES";
        HttpContext.Current.Response.Write("<Td align='center'colspan= 28>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line6);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        line6 = "DEDUCTIONS";
        HttpContext.Current.Response.Write("<Td align='center' colspan= 19>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line6);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("<Td align='left' colspan= 4>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line5);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");


        for (int j = 0; j < columnscount; j++)
        {

            HttpContext.Current.Response.Write("<Td valign='middle'>");
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.Columns[j].ToString());
            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
        }

        HttpContext.Current.Response.Write("</TR>");

        foreach (DataRow row in table.Rows)
        {//write in new row
            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row[i].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write(style);
            }

            HttpContext.Current.Response.Write("</TR>");
        }
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public void ExporttoExcelbank(DataTable table, string FileName, string heading, string line1, string line2, string line3, string line4, string line5, string line6)
    {


        // filename = "SalarySheet.xls";
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename='" + FileName + "'.xls");

        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = table.Columns.Count;

        //33

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='Left' colspan= '" + columnscount / 2 + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(heading);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("<Td align='Right' colspan= '" + columnscount / 2 + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line5);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='center' colspan= '" + columnscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line1);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='Left' colspan= '" + columnscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line2);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='Left' colspan= '" + columnscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line3);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='Left' colspan= '" + columnscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line4);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");


        for (int j = 0; j < columnscount; j++)
        {
            //write in new column

            HttpContext.Current.Response.Write("<Td valign='middle'>");

            //Get column headers  and make it as bold in excel columns
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.Columns[j].ToString());

            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
        }

        HttpContext.Current.Response.Write("</TR>");

        foreach (DataRow row in table.Rows)
        {//write in new row
            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row[i].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write(style);
            }

            HttpContext.Current.Response.Write("</TR>");
        }
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
        "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
        "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount1 = table.Columns.Count;

        //33

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='Left' colspan= '" + columnscount1 + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line6);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");


        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public void ExporttoExcelForBankUpload(string fileName, GridView gv, string line, string line1, int count)
    {
        GridViewExportUtil gve = new GridViewExportUtil();
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader(
            "content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/ms-excel";

        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        HttpContext.Current.Response.Write("<TR valign='top'>");

        HttpContext.Current.Response.Write("<Td style='border:none' align='left'  colspan= " + count / 2 + ">");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line);
        HttpContext.Current.Response.Write("</B>");

        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td style='border:none' align='right' colspan= " + count / 2 + ">");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line1);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                //  Create a form to contain the grid
                Table table = new Table();
                table.BorderStyle = BorderStyle.Solid;
                table.GridLines = GridLines.Both;
                //  add the header row to the table
                if (gv.HeaderRow != null)
                {
                    gve.PrepareControlForExport(gv.HeaderRow);
                    table.Rows.Add(gv.HeaderRow);
                }

                //  add each of the data rows to the table
                foreach (GridViewRow row in gv.Rows)
                {
                    gve.PrepareControlForExport(row);
                    table.Rows.Add(row);
                }

                //  add the footer row to the table
                if (gv.FooterRow != null)
                {
                    gve.PrepareControlForExport(gv.FooterRow);
                    table.Rows.Add(gv.FooterRow);
                }

                //  render the table into the htmlwriter
                table.RenderControl(htw);

                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Write(sw.ToString());

            }
        }
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
        "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
        "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        HttpContext.Current.Response.Write("<TR valign='top'>");

        HttpContext.Current.Response.Write("<Td style='border:none' align='left' colspan= " + count / 3 + ">");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("Prepared by");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td style='border:none' align='center' colspan= " + count / 3 + ">");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("Checked by");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td style='border:none' align='right' colspan= " + count / 3 + ">");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("Authorised by");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.End();

    }

    public void ExporttoExcelAttendancesheet(DataTable table)
    {


        string filename = "Attendancesheet.xls";
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Attendancesheet.xls");

        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = table.Columns.Count;




        for (int j = 0; j < columnscount; j++)
        {
            //write in new column

            HttpContext.Current.Response.Write("<Td valign='middle'>");

            //Get column headers  and make it as bold in excel columns
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.Columns[j].ToString());

            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
        }

        HttpContext.Current.Response.Write("</TR>");

        foreach (DataRow row in table.Rows)
        {//write in new row
            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row[i].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write(style);
            }

            HttpContext.Current.Response.Write("</TR>");
        }
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }


    public void ExporttoExcelForICICIBankUpload(string fileName, GridView gv, string line, string line1, string line2, string line3, string line4, string line5)
    {
        GridViewExportUtil gve = new GridViewExportUtil();
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader(
            "content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/ms-excel";

        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        HttpContext.Current.Response.Write("<TR valign='top'>");

        HttpContext.Current.Response.Write("<Td style='border:none' align='left'  colspan= 3>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td style='border:none' align='left'  colspan= 2>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line1);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td style='border:none' align='left' colspan= 2>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line2);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td style='border:none' align='left' colspan= 2>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line3);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR valign='top'>");

        HttpContext.Current.Response.Write("<Td style='border:none' align='left'  colspan= 3>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line4);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td style='border:none' align='left'  colspan= 6>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line5);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                //  Create a form to contain the grid
                Table table = new Table();
                table.BorderStyle = BorderStyle.Solid;
                table.GridLines = GridLines.Both;
                //  add the header row to the table
                if (gv.HeaderRow != null)
                {
                    gve.PrepareControlForExport(gv.HeaderRow);
                    table.Rows.Add(gv.HeaderRow);
                }

                //  add each of the data rows to the table
                foreach (GridViewRow row in gv.Rows)
                {
                    gve.PrepareControlForExport(row);
                    table.Rows.Add(row);
                }

                //  add the footer row to the table
                if (gv.FooterRow != null)
                {
                    gve.PrepareControlForExport(gv.FooterRow);
                    table.Rows.Add(gv.FooterRow);
                }

                //  render the table into the htmlwriter
                table.RenderControl(htw);

                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Write(sw.ToString());

            }
        }

        HttpContext.Current.Response.End();

    }

}

