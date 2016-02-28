﻿namespace Hexa.Core.Web.Mvc.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    public enum Align
    {
        center,
        left,
        right
    }

    public enum DataType
    {
        json,
        local,
        xml
    }

    public enum Direction
    {
        vertical,
        horizontal
    }

    public enum Formatters
    {
        integer,
        number,
        currency,
        date,
        email,
        link,
        showlink,
        checkbox,
        select
    }

    public enum LoadUi
    {
        enable,
        disable,
        block
    }

    public enum MultiKey
    {
        altKey,
        ctrlKey,
        shiftKey
    }

    public enum PagerPos
    {
        center,
        left,
        right
    }

    public enum RecordPos
    {
        center,
        left,
        right
    }

    public enum RequestType
    {
        get,
        post
    }

    public enum Searchtype
    {
        text,
        select,
        datepicker
    }

    public enum SortOrder
    {
        asc,
        desc
    }

    public enum ToolbarPosition
    {
        top,
        bottom,
        both
    }

    public class Column
    {
        private Align? align;
        private List<string> classes = new List<string>();
        private string columnName;
        private string customFormatter;
        private SortOrder? firstSortOrder;
        private bool? fixedWidth;
        private KeyValuePair<Formatters, string>? formatter;
        private bool? hidden;
        private string index;
        private bool? key;
        private string label;
        private bool? resizeable;
        private bool? search;
        private string searchDateFormat;
        private string[] searchTerms;
        private Searchtype? searchType;
        private bool? sortable;
        private bool? title;
        private int? width;
        private bool editable;
        private string editType;
        private string editOptions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="columnName">Name of column, cannot be blank or set to 'subgrid', 'cb', and 'rn'</param>
        public Column(string columnName)
        {
            // Make sure columnname is not left blank
            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentException("No columnname specified");
            }

            // Make sure columnname is not part of the reserved names collection
            var reservedNames = new string[] { "subgrid", "cb", "rn" };

            if (reservedNames.Contains(columnName))
            {
                throw new ArgumentException("Columnname '" + columnName + "' is reserved");
            }

            // Set columnname
            this.columnName = columnName;

            // Set index equal to columnname by default, can be overriden by setter
            this.index = columnName;
        }

        /// <summary>
        /// This option allow to add a class to to every cell on that column. In the grid css
        /// there is a predefined class ui-ellipsis which allow to attach ellipsis to a
        /// particular row. Also this will work in FireFox too.
        /// Multiple calls to this function are allowed to set multiple classes
        /// </summary>
        /// <param name="className">Classname</param>
        public Column AddClass(string className)
        {
            this.classes.Add(className);
            return this;
        }

        /// <summary>
        /// Defines the alignment of the cell in the Body layer, not in header cell.
        /// Possible values: left, center, right. (default: left)
        /// </summary>
        /// <param name="align">Alignment of column (center, right, left</param>
        public Column SetAlign(Align align)
        {
            this.align = align;
            return this;
        }

        /// <summary>
        /// Sets custom formatter. Usually this is a function. When set in the formatter option
        /// this should not be enclosed in quotes and not entered with () -
        /// just specify the name of the function
        /// The following variables are passed to the function:
        /// 'cellvalue': The value to be formated (pure text).
        /// 'options': Object { rowId: rid, colModel: cm} where rowId - is the id of the row colModel is
        /// the object of the properties for this column getted from colModel array of jqGrid
        /// 'rowobject': Row data represented in the format determined from datatype option.
        /// If we have datatype: xml/xmlstring - the rowObject is xml node,provided according to the rules
        /// from xmlReader If we have datatype: json/jsonstring - the rowObject is array, provided according to
        /// the rules from jsonReader
        /// </summary>
        /// <param name="customFormatter"></param>
        /// <returns></returns>
        public Column SetCustomFormatter(string customFormatter)
        {
            if (this.formatter.HasValue)
            {
                throw new Exception("You cannot set a formatter and a customformatter at the same time, please choose one.");
            }

            this.customFormatter = customFormatter;
            return this;
        }

        /// <summary>
        /// Marks the columns as editable.
        /// </summary>
        /// <returns>the current column</returns>
        public Column SetEditable()
        {
            this.editable = true;
            return this;
        }

        /// <summary>
        /// Sets the edit type of the column
        /// </summary>
        /// <returns>the current column</returns>
        public Column SetEditType(string editType)
        {
            this.editType = editType;
            return this;
        }

        /// <summary>
        /// Sets the edit options of the column
        /// </summary>
        /// <returns>the current column</returns>
        public Column SetEditOptions(string options)
        {
            this.editOptions= options;
            return this;
        }

        /// <summary>
        /// If set to asc or desc, the column will be sorted in that direction on first
        /// sort.Subsequent sorts of the column will toggle as usual (default: null)
        /// </summary>
        /// <param name="firstSortOrder">First sort order</param>
        public Column SetFirstSortOrder(SortOrder firstSortOrder)
        {
            this.firstSortOrder = firstSortOrder;
            return this;
        }

        /// <summary>
        /// If set to true this option does not allow recalculation of the width of the
        /// column if shrinkToFit option is set to true. Also the width does not change
        /// if a setGridWidth method is used to change the grid width. (default: false)
        /// </summary>
        /// <param name="fixedWidth">Indicates if width of column is fixed</param>
        public Column SetFixed(bool fixedWidth)
        {
            this.fixedWidth = fixedWidth;
            return this;
        }

        /// <summary>
        /// Sets formatter with default formatoptions (as set in language file)
        /// </summary>
        /// <param name="formatter">Formatter</param>
        public Column SetFormatter(Formatters formatter)
        {
            if (!string.IsNullOrWhiteSpace(this.customFormatter))
            {
                throw new Exception("You cannot set a formatter and a customformatter at the same time, please choose one.");
            }

            this.formatter = new KeyValuePair<Formatters, string>(formatter, string.Empty);
            return this;
        }

        /// <summary>
        /// Sets formatter with formatoptions (see jqGrid documentation for more info on formatoptions)
        /// </summary>
        /// <param name="formatter">Formatter</param>
        /// <param name="formatOptions">Formatoptions</param>
        public Column SetFormatter(Formatters formatter, string formatOptions)
        {
            if (!string.IsNullOrWhiteSpace(this.customFormatter))
            {
                throw new Exception("You cannot set a formatter and a customformatter at the same time, please choose one.");
            }

            this.formatter = new KeyValuePair<Formatters, string>(formatter, formatOptions);
            return this;
        }

        /// <summary>
        /// Defines if this column is hidden at initialization. (default: false)
        /// </summary>
        /// <param name="hidden">Boolean indicating if column is hidden</param>
        public Column SetHidden(bool hidden)
        {
            this.hidden = hidden;
            return this;
        }

        /// <summary>
        /// Set the index name when sorting. Passed as sidx parameter. (default: Same as columnname)
        /// </summary>
        /// <param name="index">Name of index</param>
        public Column SetIndex(string index)
        {
            this.index = index;
            return this;
        }

        /// <summary>
        /// In case if there is no id from server, this can be set as as id for the unique row id.
        /// Only one column can have this property. If there are more than one key the grid finds
        /// the first one and the second is ignored. (default: false)
        /// </summary>
        /// <param name="key">Indicates if key is set</param>
        public Column SetKey(bool key)
        {
            this.key = key;
            return this;
        }

        /// <summary>
        /// Defines the heading for this column. If empty, the heading for this column comes from the name property.
        /// </summary>
        /// <param name="label">Label name of column</param>
        public Column SetLabel(string label)
        {
            this.label = label;
            return this;
        }

        /// <summary>
        /// Defines if the column can be resized (default: true)
        /// </summary>
        /// <param name="resizeable">Indicates if the column is resizable</param>
        public Column SetResizeable(bool resizeable)
        {
            this.resizeable = resizeable;
            return this;
        }

        /// <summary>
        /// When used in search modules, disables or enables searching on that column. (default: true)
        /// </summary>
        /// <param name="search">Indicates if searching for this column is enabled</param>
        public Column SetSearch(bool search)
        {
            this.search = search;
            return this;
        }

        /// <summary>
        /// Set dateformat of datepicker when searchtype is set to datepicker (default: dd-mm-yy)
        /// </summary>
        /// <param name="searchDateFormat">Dateformat</param>
        public Column SetSearchDateFormat(string searchDateFormat)
        {
            this.searchDateFormat = searchDateFormat;
            return this;
        }

        /// <summary>
        /// Set searchterms if search type of this column is set to type select
        /// </summary>
        /// <param name="searchTerms">Searchterm to add to dropdownlist</param>
        public Column SetSearchTerms(string[] searchTerms)
        {
            this.searchTerms = searchTerms;
            return this;
        }

        /// <summary>
        /// Sets the searchtype of this column (text, select or datepicker) (default: text)
        /// Note: To use datepicker jQueryUI javascript should be included
        /// </summary>
        /// <param name="searchType">Search type</param>
        public Column SetSearchType(Searchtype searchType)
        {
            this.searchType = searchType;
            return this;
        }

        /// <summary>
        /// Indicates if column is sortable (default: true)
        /// </summary>
        /// <param name="sortable">Indicates if column is sortable</param>
        public Column SetSortable(bool sortable)
        {
            this.sortable = sortable;
            return this;
        }

        /// <summary>
        /// If this option is false the title is not displayed in that column when we hover over a cell (default: true)
        /// </summary>
        /// <param name="title">Indicates if title is displayed when hovering over cell</param>
        public Column SetTitle(bool title)
        {
            this.title = title;
            return this;
        }

        /// <summary>
        /// Set the initial width of the column, in pixels. This value currently can not be set as percentage (default: 150)
        /// </summary>
        /// <param name="width">Width in pixels</param>
        public Column SetWidth(int width)
        {
            this.width = width;
            return this;
        }

        /// <summary>
        /// Creates javascript string from column to be included in grid javascript
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var script = new StringBuilder();

            // Start column
            script.Append("{").AppendLine();

            // Align
            if (this.align.HasValue)
            {
                script.AppendFormat("align: '{0}',", this.align).AppendLine();
            }

            // Classes
            if (this.classes.Count > 0)
            {
                script.AppendFormat("classes: '{0}',", string.Join(" ", (from c in this.classes select c).ToArray())).AppendLine();
            }

            // Columnname
            script.AppendFormat("name:'{0}',", this.columnName).AppendLine();

            // FirstSortOrder
            if (this.firstSortOrder.HasValue)
            {
                script.AppendFormat("firstsortorder: '{0}',", this.firstSortOrder).AppendLine();
            }

            // FixedWidth
            if (this.fixedWidth.HasValue)
            {
                script.AppendFormat("fixed: {0},", this.fixedWidth.Value.ToString().ToLower()).AppendLine();
            }

            // Formatters
            if (this.formatter.HasValue && string.IsNullOrWhiteSpace(this.formatter.Value.Value))
            {
                script.AppendFormat("formatter: '{0}',", this.formatter.Value.Key).AppendLine();
            }

            if (this.formatter.HasValue && !string.IsNullOrWhiteSpace(this.formatter.Value.Value))
            {
                script.AppendLine("formatter: '" + this.formatter.Value.Key + "', formatoption: {" + this.formatter.Value.Value + "} ,");
            }

            // Custom formatter
            if (!string.IsNullOrWhiteSpace(this.customFormatter))
            {
                script.AppendFormat("formatter: {0},", this.customFormatter).AppendLine();
            }

            // Hidden
            if (this.hidden.HasValue)
            {
                script.AppendFormat("hidden: {0},", this.hidden.Value.ToString().ToLower()).AppendLine();
            }

            // Key
            if (this.key.HasValue)
            {
                script.AppendFormat("key: {0},", this.key.Value.ToString().ToLower()).AppendLine();
            }

            // Label
            if (!string.IsNullOrWhiteSpace(this.label))
            {
                script.AppendFormat("label: '{0}',", this.label).AppendLine();
            }

            // Resizable
            if (this.resizeable.HasValue)
            {
                script.AppendFormat("resizable: {0},", this.resizeable.Value.ToString().ToLower()).AppendLine();
            }

            // Search
            if (this.search.HasValue)
            {
                script.AppendFormat("search: {0},", this.search.Value.ToString().ToLower()).AppendLine();
            }

            // SearchType
            if (this.searchType.HasValue)
            {
                if (this.searchType.Value == Searchtype.text)
                {
                    script.AppendLine("stype:'text',");
                }

                if (this.searchType.Value == Searchtype.select)
                {
                    script.AppendLine("stype:'select',");
                }
            }

            // Searchoptions
            if (this.searchType == Searchtype.select || this.searchType == Searchtype.datepicker || this.searchType == Searchtype.text)
            {
                script.Append("searchoptions: {");

                // Searchtype select
                if (this.searchType == Searchtype.select || this.searchType == Searchtype.text)
                {
                    if (this.searchTerms != null)
                    {
                        script.AppendFormat("sopt: [{0}]", string.Join(",", from s in this.searchTerms select "'" + s + "'"));
                    }
                    else
                    {
                        script.Append("value: ':'");
                    }
                }

                // Searchtype datepicker
                if (this.searchType == Searchtype.datepicker)
                {
                    if (this.searchTerms != null)
                    {
                        script.AppendFormat("sopt: [{0}],", string.Join(",", from s in this.searchTerms select "'" + s + "'"));
                    }

                    if (string.IsNullOrWhiteSpace(this.searchDateFormat))
                    {
                        script.Append("dataInit:function(el){$(el).datepicker({changeYear:true, onChange: function(formated, dates){ $(el).val(formated);},dateFormat:'dd-mm-yy'});}");
                    }
                    else
                    {
                        script.Append("dataInit:function(el){$(el).datepicker({changeYear:true, onChange: function(formated, dates){ $(el).val(formated);},dateFormat:'" + this.searchDateFormat + "'});}");
                    }
                }

                script.AppendLine("},");
            }

            // Sortable
            if (this.sortable.HasValue)
            {
                script.AppendFormat("sortable: {0},", this.sortable.Value.ToString().ToLower()).AppendLine();
            }

            // Title
            if (this.title.HasValue)
            {
                script.AppendFormat("title: {0},", this.title.Value.ToString().ToLower()).AppendLine();
            }

            // Width
            if (this.width.HasValue)
            {
                script.AppendFormat("width:{0},", this.width.Value).AppendLine();
            }

            if (this.editable)
            {
                script.AppendLine("editable:true,").AppendLine();
                if (!string.IsNullOrWhiteSpace(this.editType))
                {
                    script.AppendFormat("edittype:'{0}',", this.editType).AppendLine();
                }
                if (!string.IsNullOrWhiteSpace(this.editOptions))
                {
                    script.AppendFormat("editoptions:{0},", this.editOptions).AppendLine();
                }
            }

            // Index
            script.AppendFormat("index:'{0}'", this.index).AppendLine();

            // End column
            script.Append("}");

            return script.ToString();
        }
    }

    /// <summary>
    /// Grid class, used to render JqGrid
    /// </summary>
    public class Grid
    {
        private string altClass;
        private bool? altRows;
        private bool? autoEncode;
        private bool? autoWidth;
        private string caption;
        private bool closeAfterSearch;
        private bool closeOnEscape;
        private List<Column> columns = new List<Column>();
        private DataType dataType = DataType.json;
        private string emptyRecords;
        private bool? footerRow;
        private bool? forceFit;
        private bool? gridView;
        private bool groupCollapse;
        private List<bool> groupColumnShow = new List<bool>();
        private List<string> groupFields = new List<string>();
        private bool grouping;
        private List<string> groupOrder = new List<string>();
        private List<bool> groupSummary = new List<bool>();
        private string groupText;
        private bool? headerTitles;
        private int? height;
        private bool? hiddenGrid;
        private bool? hideGrid;
        private bool? hoverRows;
        private string id;
        private bool? loadOnce;
        private string loadText;
        private LoadUi? loadUI;
        private bool? multiBoxOnly;
        private MultiKey? multiKey;
        private bool multipleSearch;
        private bool? multiSelect;
        private int? multiSelectWidth;
        private string onAfterInsertRow;
        private string onBeforeRequest;
        private string onBeforeSelectRow;
        private string onCellSelect;
        private string onDblClickRow;
        private string onGridComplete;
        private string onHeaderClick;
        private string onLoadBeforeSend;
        private string onLoadComplete;
        private string onLoadError;
        private string onPaging;
        private string onResizeStart;
        private string onResizeStop;
        private string onRightClickRow;
        private string onSelectAll;
        private string onSelectRow;
        private string onSerializeGridData;
        private string onSortCol;
        private int? page;
        private string pager;
        private PagerPos? pagerPos;
        private bool? pgButtons;
        private bool? pgInput;
        private string pgText;
        private RecordPos? recordPos;
        private string recordText;
        private RequestType? requestType;
        private string resizeClass;
        private int[] rowList;
        private int? rowNum;
        private bool? rowNumbers;
        private int? rowNumWidth;
        private bool? scroll;
        private int? scrollInt;
        private int? scrollOffset;
        private bool? scrollRows;
        private int? scrollTimeout;
        private bool search;
        private bool? searchClearButton;
        private bool? searchOnEnter;
        private bool? searchToggleButton;
        private bool? searchToolbar;
        private bool? showAllSortIcons;
        private bool? shrinkToFit;
        private Direction? sortIconDirection;
        private string sortName;
        private bool? sortOnHeaderClick;
        private SortOrder? sortOrder;
        private bool? toolbar;
        private ToolbarPosition toolbarPosition = ToolbarPosition.top;
        private bool? topPager;
        private string url;
        private bool? viewRecords;
        private int? width;
        private bool edit;
        private bool add;
        private bool delete;
        private string data;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Id of grid</param>
        public Grid(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Id must contain a value to identify the grid");
            }

            this.id = id;
        }

        /// <summary>
        /// Enables the Add button
        /// </summary>
        /// <returns>Current Grid instance</returns>
        public Grid Add()
        { 
            this.add = true;
            return this;
        }

        /// <summary>
        /// Adds columns to grid
        /// </summary>
        /// <param name="column">Colomn object</param>
        public Grid AddColumn(Column column)
        {
            this.columns.Add(column);
            return this;
        }

        /// <summary>
        /// Enables the Delete button
        /// </summary>
        /// <returns>Current Grid instance</returns>
        public Grid Delete()
        {
            this.delete = true;
            return this;
        }

        /// <summary>
        /// Enables the Edit button
        /// </summary>
        /// <returns>Current Grid instance</returns>
        public Grid Edit()
        {
            this.edit = true;
            return this;
        }

        /// <summary>
        /// Tells if the group must be or not collapsed
        /// </summary>
        /// <param name="collapse">if set to <c>true</c> [collapse].</param>
        /// <returns></returns>
        public Grid GroupCollapse(bool collapse)
        {
            this.groupCollapse = collapse;
            return this;
        }

        /// <summary>
        /// Tells if the grouping column must be shown.
        /// </summary>
        /// <param name="show">if set to <c>true</c> [show].</param>
        /// <returns></returns>
        public Grid GroupColumnShow(bool show)
        {
            this.groupColumnShow.Add(show);
            return this;
        }

        /// <summary>
        /// Enables grouping
        /// </summary>
        /// <returns></returns>
        public Grid Grouping()
        {
            this.grouping = true;
            return this;
        }

        /// <summary>
        /// Tell what field will be used for grouping
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public Grid GroupingField(string field)
        {
            this.groupFields.Add(field);
            return this;
        }

        /// <summary>
        /// Tells if the group must be orderded asc or desc.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        public Grid GroupOrder(string order)
        {
            this.groupOrder.Add(order);
            return this;
        }

        /// <summary>
        /// Tells if the group must show or not a summary.
        /// </summary>
        /// <param name="summary">if set to <c>true</c> [summary].</param>
        /// <returns></returns>
        public Grid GroupSummary(bool summary)
        {
            this.groupSummary.Add(summary);
            return this;
        }

        /// <summary>
        /// The text format for the gruuping. i.e. (['<b></b>'])
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public Grid GroupText(string text)
        {
            this.groupText = text;
            return this;
        }

        /// <summary>
        /// This event fires after each inserted row.
        /// Variables available in call:
        /// 'rowid': Id of the inserted row
        /// 'rowdata': An array of the data to be inserted into the row. This is array of type name-
        /// value, where the name is a name from colModel
        /// 'rowelem': The element from the response. If the data is xml this is the xml element of the row;
        /// if the data is json this is array containing all the data for the row
        /// Note: this event does not fire if gridview option is set to true
        /// </summary>
        /// <param name="onAfterInsertRow">Script to be executed</param>
        public Grid OnAfterInsertRow(string onAfterInsertRow)
        {
            this.onAfterInsertRow = onAfterInsertRow;
            return this;
        }

        /// <summary>
        /// This event fires before requesting any data. Does not fire if datatype is function
        /// Variables available in call: None
        /// </summary>
        /// <param name="onBeforeRequest">Script to be executed</param>
        public Grid OnBeforeRequest(string onBeforeRequest)
        {
            this.onBeforeRequest = onBeforeRequest;
            return this;
        }

        /// <summary>
        /// This event fires when the user clicks on the row, but before selecting it.
        /// Variables available in call:
        /// 'rowid': The id of the row.
        /// 'e': The event object
        /// This event should return boolean true or false. If the event returns true the selection
        /// is done. If the event returns false the row is not selected and any other action if defined
        /// does not occur.
        /// </summary>
        /// <param name="onBeforeSelectRow">Script to be executed</param>
        public Grid OnBeforeSelectRow(string onBeforeSelectRow)
        {
            this.onBeforeSelectRow = onBeforeSelectRow;
            return this;
        }

        /// <summary>
        /// Fires when we click on a particular cell in the grid.
        /// Variables available in call:
        /// 'rowid': The id of the row
        /// 'iCol': The index of the cell,
        /// 'cellcontent': The content of the cell,
        /// 'e': The event object element where we click.
        /// (Note that this available when we are not using cell editing module
        /// and is disabled when using cell editing).
        /// </summary>
        /// <param name="onCellSelect">Script to be executed</param>
        public Grid OnCellSelect(string onCellSelect)
        {
            this.onCellSelect = onCellSelect;
            return this;
        }

        /// <summary>
        /// Raised immediately after row was double clicked.
        /// Variables available in call:
        /// 'rowid': The id of the row,
        /// 'iRow': The index of the row (do not mix this with the rowid),
        /// 'iCol': The index of the cell.
        /// 'e': The event object
        /// </summary>
        /// <param name="onDblClickRow">Script to be executed</param>
        public Grid OnDblClickRow(string onDblClickRow)
        {
            this.onDblClickRow = onDblClickRow;
            return this;
        }

        /// <summary>
        /// This fires after all the data is loaded into the grid and all the other processes are complete.
        /// Also the event fires independent from the datatype parameter and after sorting paging and etc.
        /// Variables available in call: None
        /// </summary>
        /// <param name="onGridComplete">Script to be executed</param>
        public Grid OnGridComplete(string onGridComplete)
        {
            this.onGridComplete = onGridComplete;
            return this;
        }

        /// <summary>
        /// Fires after clicking hide or show grid (hidegrid:true)
        /// Variables available in call:
        /// 'gridstate': The state of the grid - can have two values - visible or hidden
        /// </summary>
        /// <param name="onHeaderClick">Script to be executed</param>
        public Grid OnHeaderClick(string onHeaderClick)
        {
            this.onHeaderClick = onHeaderClick;
            return this;
        }

        /// <summary>
        /// A pre-callback to modify the XMLHttpRequest object (xhr) before it is sent. Use this to set
        /// custom headers etc. The XMLHttpRequest is passed as the only argument.
        /// Variables available in call:
        /// 'xhr': The XMLHttpRequest
        /// </summary>
        /// <param name="onLoadBeforeSend">Script to be executed</param>
        public Grid OnLoadBeforeSend(string onLoadBeforeSend)
        {
            this.onLoadBeforeSend = onLoadBeforeSend;
            return this;
        }

        /// <summary>
        /// This event is executed immediately after every server request.
        /// Variables available in call:
        /// 'xhr': The XMLHttpRequest
        /// </summary>
        /// <param name="onLoadComplete">Script to be executed</param>
        public Grid OnLoadComplete(string onLoadComplete)
        {
            this.onLoadComplete = onLoadComplete;
            return this;
        }

        /// <summary>
        /// A function to be called if the request fails.
        ///  Variables available in call:
        ///  'xhr': The XMLHttpRequest
        ///  'status': String describing the type of error
        ///  'error': Optional exception object, if one occurred
        /// </summary>
        /// <param name="onLoadError">Script to be executed</param>
        public Grid OnLoadError(string onLoadError)
        {
            this.onLoadError = onLoadError;
            return this;
        }

        /// <summary>
        /// This event fires after click on [page button] and before populating the data.
        /// Also works when the user enters a new page number in the page input box
        /// (and presses [Enter]) and when the number of requested records is changed via
        /// the select box.
        /// If this event returns 'stop' the processing is stopped and you can define your
        /// own custom paging
        /// Variables available in call:
        /// 'pgButton': first,last,prev,next in case of button click, records in case when
        /// a number of requested rows is changed and user when the user change the number of the requested page
        /// </summary>
        /// <param name="onPaging">Script to be executed</param>
        public Grid OnPaging(string onPaging)
        {
            this.onPaging = onPaging;
            return this;
        }

        /// <summary>
        /// Event which is called when we start resizing a column.
        /// Variables available in call:
        /// 'event':  The event object
        /// 'index': The index of the column in colModel.
        /// </summary>
        /// <param name="onResizeStart">Script to be executed</param>
        public Grid OnResizeStart(string onResizeStart)
        {
            this.onResizeStart = onResizeStart;
            return this;
        }

        /// <summary>
        /// Event which is called after the column is resized.
        /// Variables available in call:
        /// 'newwidth': The new width of the column
        /// 'index': The index of the column in colModel.
        /// </summary>
        /// <param name="onResizeStop">Script to be executed</param>
        public Grid OnResizeStop(string onResizeStop)
        {
            this.onResizeStop = onResizeStop;
            return this;
        }

        /// <summary>
        /// Raised immediately after row was right clicked.
        /// Variables available in call:
        /// 'rowid': The id of the row,
        /// 'iRow': The index of the row (do not mix this with the rowid),
        /// 'iCol': The index of the cell.
        /// 'e': The event object
        /// Note - this event does not work in Opera browsers, since Opera does not support oncontextmenu event
        /// </summary>
        /// <param name="onRightClickRow">Script to be executed</param>
        public Grid OnRightClickRow(string onRightClickRow)
        {
            this.onRightClickRow = onRightClickRow;
            return this;
        }

        /// <summary>
        /// This event fires when multiselect option is true and you click on the header checkbox.
        /// Variables available in call:
        /// 'aRowids':  Array of the selected rows (rowid's).
        /// 'status': Boolean variable determining the status of the header check box - true if checked, false if not checked.
        /// Note that the aRowids alway contain the ids when header checkbox is checked or unchecked.
        /// </summary>
        /// <param name="onSelectAll">Script to be executed</param>
        public Grid OnSelectAll(string onSelectAll)
        {
            this.onSelectAll = onSelectAll;
            return this;
        }

        /// <summary>
        /// Raised immediately when row is clicked.
        /// Variables available in function call:
        /// 'rowid': The id of the row,
        /// 'status': Tthe status of the selection. Can be used when multiselect is set to true.
        /// true if the row is selected, false if the row is deselected.
        /// <param name="onSelectRow">Script to be executed</param>
        /// </summary>
        public Grid OnSelectRow(string onSelectRow)
        {
            this.onSelectRow = onSelectRow;
            return this;
        }

        /// <summary>
        /// If this event is set it can serialize the data passed to the ajax request.
        /// The function should return the serialized data. This event can be used when
        /// custom data should be passed to the server - e.g - JSON string, XML string and etc.
        /// Variables available in call:
        /// 'postData': Posted data
        /// </summary>
        /// <param name="onSerializeGridData">Script to be executed</param>
        public Grid OnSerializeGridData(string onSerializeGridData)
        {
            this.onSerializeGridData = onSerializeGridData;
            return this;
        }

        /// <summary>
        /// Raised immediately after sortable column was clicked and before sorting the data.
        /// Variables available in call:
        /// 'index': The index name from colModel
        /// 'iCol': The index of column,
        /// 'sortorder': The new sorting order - can be 'asc' or 'desc'.
        /// If this event returns 'stop' the sort processing is stopped and you can define your own custom sorting
        /// </summary>
        /// <param name="onSortCol">Script to be executed</param>
        public Grid OnSortCol(string onSortCol)
        {
            this.onSortCol = onSortCol;
            return this;
        }

        /// <summary>
        /// Renders this instance.
        /// </summary>
        /// <returns></returns>
        public MvcHtmlString Render()
        {
            return new MvcHtmlString(this.ToString());
        }

        /// <summary>
        /// The class that is used for alternate rows. You can construct your own class and replace this value.
        /// This option is valid only if altRows options is set to true (default: ui-priority-secondary)
        /// </summary>
        /// <param name="altClass">Classname for alternate rows</param>
        public Grid SetAltClass(string altClass)
        {
            this.altClass = altClass;
            return this;
        }

        /// <summary>
        /// Set a zebra-striped grid (default: false)
        /// </summary>
        /// <param name="altRows">Boolean indicating if zebra-striped grid is used</param>
        public Grid SetAltRows(bool altRows)
        {
            this.altRows = altRows;
            return this;
        }

        /// <summary>
        /// When set to true encodes (html encode) the incoming (from server) and posted
        /// data (from editing modules). For example < will be converted to &lt (default: false)
        /// </summary>
        /// <param name="autoEncode">Boolean indicating if autoencode is used</param>
        public Grid SetAutoEncode(bool autoEncode)
        {
            this.autoEncode = autoEncode;
            return this;
        }

        /// <summary>
        /// When set to true, the grid width is recalculated automatically to the width of the
        /// parent element. This is done only initially when the grid is created. In order to
        /// resize the grid when the parent element changes width you should apply custom code
        /// and use a setGridWidth method for this purpose. (default: false)
        /// </summary>
        /// <param name="autoWidth">Boolean indicating if autowidth is used</param>
        public Grid SetAutoWidth(bool autoWidth)
        {
            this.autoWidth = autoWidth;
            return this;
        }

        /// <summary>
        /// Defines the caption layer for the grid. This caption appears above the header layer.
        /// If the string is empty the caption does not appear. (default: empty)
        /// </summary>
        /// <param name="caption">Caption of grid</param>
        public Grid SetCaption(string caption)
        {
            this.caption = caption;
            return this;
        }

        /// <summary>
        /// If true searchbox closes after searh.
        /// </summary>
        /// <param name="closeAfterSearch">if set to <c>true</c> [close after search].</param>
        /// <returns></returns>
        public Grid SetCloseAfterSearch(bool closeAfterSearch)
        {
            this.closeAfterSearch = closeAfterSearch;
            return this;
        }

        /// <summary>
        /// If true search box closes on escape pressed.
        /// </summary>
        /// <param name="closeOnEscape">if set to <c>true</c> [close on escape].</param>
        /// <returns></returns>
        public Grid SetCloseOnEscape(bool closeOnEscape)
        {
            this.closeOnEscape = closeOnEscape;
            return this;
        }

        /// <summary>
        /// Gives the grid the data when the data type is local
        /// </summary>
        /// <param name="data">the data</param>
        public Grid SetData(string data)
        {
            this.data = data;
            return this;
        }

        /// <summary>
        /// Defines what type of information to expect to represent data in the grid. Valid
        /// options are json (default) and xml
        /// </summary>
        /// <param name="dataType">Data type</param>
        public Grid SetDataType(DataType dataType)
        {
            this.dataType = dataType;
            return this;
        }

        /// <summary>
        /// Displayed when the returned (or the current) number of records is zero.
        /// This option is valid only if viewrecords option is set to true. (default value is
        /// set in language file)
        /// </summary>
        /// <param name="emptyRecords">Display string</param>
        public Grid SetEmptyRecords(string emptyRecords)
        {
            this.emptyRecords = emptyRecords;
            return this;
        }

        /// <summary>
        /// If set to true this will place a footer table with one row below the grid records
        /// and above the pager. The number of columns equal to the number of columns in the colModel
        /// (default: false)
        /// </summary>
        /// <param name="footerRow">Boolean indicating whether footerrow is displayed</param>
        public Grid SetFooterRow(bool footerRow)
        {
            this.footerRow = footerRow;
            return this;
        }

        /// <summary>
        /// If set to true, when resizing the width of a column, the adjacent column (to the right)
        /// will resize so that the overall grid width is maintained (e.g., reducing the width of
        /// column 2 by 30px will increase the size of column 3 by 30px).
        /// In this case there is no horizontal scrolbar.
        /// Note: this option is not compatible with shrinkToFit option - i.e if
        /// shrinkToFit is set to false, forceFit is ignored.
        /// </summary>
        /// <param name="forceFit">Boolean indicating if forcefit is enforced</param>
        public Grid SetForceFit(bool forceFit)
        {
            this.forceFit = forceFit;
            return this;
        }

        /// <summary>
        /// In the previous versions of jqGrid including 3.4.X,reading relatively big data sets
        /// (Rows >=100 ) caused speed problems. The reason for this was that as every cell was
        /// inserted into the grid we applied about 5-6 jQuery calls to it. Now this problem has
        /// been resolved; we now insert the entry row at once with a jQuery append. The result is
        /// impressive - about 3-5 times faster. What will be the result if we insert all the
        /// data at once? Yes, this can be done with help of the gridview option. When set to true,
        /// the result is a grid that is 5 to 10 times faster. Of course when this option is set
        /// to true we have some limitations. If set to true we can not use treeGrid, subGrid,
        /// or afterInsertRow event. If you do not use these three options in the grid you can
        /// set this option to true and enjoy the speed. (default: false)
        /// </summary>
        /// <param name="gridView">Boolean indicating gridview is enabled</param>
        public Grid SetGridView(bool gridView)
        {
            this.gridView = gridView;
            return this;
        }

        /// <summary>
        /// If the option is set to true the title attribute is added to the column headers (default: false)
        /// </summary>
        /// <param name="headerTitles">Boolean indicating if headertitles are enabled</param>
        public Grid SetHeaderTitles(bool headerTitles)
        {
            this.headerTitles = headerTitles;
            return this;
        }

        /// <summary>
        /// The height of the grid in pixels (default: 100%, which is the only acceptable percentage for jqGrid)
        /// </summary>
        /// <param name="height">Height in pixels</param>
        public Grid SetHeight(int height)
        {
            this.height = height;
            return this;
        }

        /// <summary>
        /// If set to true the grid initially is hidden. The data is not loaded (no request is sent) and only the
        /// caption layer is shown. When the show/hide button is clicked for the first time to show the grid, the request
        /// is sent to the server, the data is loaded, and the grid is shown. From this point on we have a regular grid.
        /// This option has effect only if the caption property is not empty. (default: false)
        /// </summary>
        /// <param name="hiddenGrid">Boolean indicating if hiddengrid is enforced</param>
        public Grid SetHiddenGrid(bool hiddenGrid)
        {
            this.hiddenGrid = hiddenGrid;
            return this;
        }

        /// <summary>
        /// Enables or disables the show/hide grid button, which appears on the right side of the caption layer.
        /// Takes effect only if the caption property is not an empty string. (default: true)
        /// </summary>
        /// <param name="hideGrid">Boolean indicating if show/hide button is enabled</param>
        public Grid SetHideGrid(bool hideGrid)
        {
            this.hideGrid = hideGrid;
            return this;
        }

        /// <summary>
        /// When set to false the mouse hovering is disabled in the grid data rows. (default: true)
        /// </summary>
        /// <param name="hoverRows">Indicates whether hoverrows is enabled</param>
        public Grid SetHoverRows(bool hoverRows)
        {
            this.hoverRows = hoverRows;
            return this;
        }

        /// <summary>
        /// If this flag is set to true, the grid loads the data from the server only once (using the
        /// appropriate datatype). After the first request the datatype parameter is automatically
        /// changed to local and all further manipulations are done on the client side. The functions
        /// of the pager (if present) are disabled. (default: false)
        /// </summary>
        /// <param name="loadOnce">Boolean indicating if loadonce is enforced</param>
        public Grid SetLoadOnce(bool loadOnce)
        {
            this.loadOnce = loadOnce;
            return this;
        }

        /// <summary>
        /// The text which appears when requesting and sorting data. This parameter override the value located
        /// in the language file
        /// </summary>
        /// <param name="loadText">Loadtext</param>
        public Grid SetLoadText(string loadText)
        {
            this.loadText = loadText;
            return this;
        }

        /// <summary>
        /// This option controls what to do when an ajax operation is in progress.
        /// 'disable' - disables the jqGrid progress indicator. This way you can use your own indicator.
        /// 'enable' (default) - enables “Loading” message in the center of the grid.
        /// 'block' - enables the “Loading” message and blocks all actions in the grid until the ajax request
        /// is finished. Note that this disables paging, sorting and all actions on toolbar, if any.
        /// </summary>
        /// <param name="loadUI">Load ui mode</param>
        public Grid SetLoadUI(LoadUi loadUI)
        {
            this.loadUI = loadUI;
            return this;
        }

        /// <summary>
        /// This option works only when multiselect = true. When multiselect is set to true, clicking anywhere
        /// on a row selects that row; when multiboxonly is also set to true, the multiselection is done only
        /// when the checkbox is clicked (Yahoo style). Clicking in any other row (suppose the checkbox is
        /// not clicked) deselects all rows and the current row is selected. (default: false)
        /// </summary>
        /// <param name="multiBoxOnly">Boolean indicating if multiboxonly is enforced</param>
        public Grid SetMultiBoxOnly(bool multiBoxOnly)
        {
            this.multiBoxOnly = multiBoxOnly;
            return this;
        }

        /// <summary>
        /// This parameter makes sense only when multiselect option is set to true.
        /// Defines the key which will be pressed
        /// when we make a multiselection. The possible values are:
        /// 'shiftKey' - the user should press Shift Key
        /// 'altKey' - the user should press Alt Key
        /// 'ctrlKey' - the user should press Ctrl Key
        /// </summary>
        /// <param name="multiKey">Key to multiselect</param>
        public Grid SetMultiKey(MultiKey multiKey)
        {
            this.multiKey = multiKey;
            return this;
        }

        /// <summary>
        /// If true enables multipleple search
        /// </summary>
        /// <param name="multipleSearch">if set to <c>true</c> [multiple search].</param>
        /// <returns></returns>
        public Grid SetMultipleSearch(bool multipleSearch)
        {
            this.multipleSearch = multipleSearch;
            return this;
        }

        /// <summary>
        /// If this flag is set to true a multi selection of rows is enabled. A new column
        /// at the left side is added. Can be used with any datatype option. (default: false)
        /// </summary>
        /// <param name="multiSelect">Boolean indicating if multiselect is enabled</param>
        public Grid SetMultiSelect(bool multiSelect)
        {
            this.multiSelect = multiSelect;
            return this;
        }

        /// <summary>
        /// Determines the width of the multiselect column if multiselect is set to true. (default: 20)
        /// </summary>
        /// <param name="multiSelectWidth"></param>
        public Grid SetMultiSelectWidth(int multiSelectWidth)
        {
            this.multiSelectWidth = multiSelectWidth;
            return this;
        }

        /// <summary>
        /// Set the initial number of selected page when we make the request.This parameter is passed to the url
        /// for use by the server routine retrieving the data (default: 1)
        /// </summary>
        /// <param name="page">Number of page</param>
        public Grid SetPage(int page)
        {
            this.page = page;
            return this;
        }

        /// <summary>
        /// If pagername is specified a pagerelement is automatically added to the grid
        /// </summary>
        /// <param name="pager">Id/name of pager</param>
        public Grid SetPager(string pager)
        {
            this.pager = pager;
            return this;
        }

        /// <summary>
        /// Determines the position of the pager in the grid. By default the pager element
        /// when created is divided in 3 parts (one part for pager, one part for navigator
        /// buttons and one part for record information) (default: center)
        /// </summary>
        /// <param name="pagerPos">Position of pager</param>
        public Grid SetPagerPos(PagerPos pagerPos)
        {
            this.pagerPos = pagerPos;
            return this;
        }

        /// <summary>
        /// Determines if the pager buttons should be displayed if pager is available. Valid
        /// only if pager is set correctly. The buttons are placed in the pager bar. (default: true)
        /// </summary>
        /// <param name="pgButtons">Boolean indicating if pager buttons are displayed</param>
        public Grid SetPgButtons(bool pgButtons)
        {
            this.pgButtons = pgButtons;
            return this;
        }

        /// <summary>
        /// Determines if the input box, where the user can change the number of the requested page,
        /// should be available. The input box appears in the pager bar. (default: true)
        /// </summary>
        /// <param name="pgInput">Boolean indicating if pager input is available</param>
        public Grid SetPgInput(bool pgInput)
        {
            this.pgInput = pgInput;
            return this;
        }

        /// <summary>
        /// Show information about current page status. The first value is the current loaded page.
        /// The second value is the total number of pages (default is set in language file)
        /// Example: "Page {0} of {1}"
        /// </summary>
        /// <param name="pgText">Current page status text</param>
        public Grid SetPgText(string pgText)
        {
            this.pgText = pgText;
            return this;
        }

        /// <summary>
        /// Determines the position of the record information in the pager. Can be left, center, right
        /// (default: right)
        /// Warning: When pagerpos en recordpos are equally set, pager is hidden.
        /// </summary>
        /// <param name="recordPos">Position of record information</param>
        public Grid SetRecordPos(RecordPos recordPos)
        {
            this.recordPos = recordPos;
            return this;
        }

        /// <summary>
        /// Represent information that can be shown in the pager. This option is valid if viewrecords
        /// option is set to true. This text appears only if the total number of records is greater then
        /// zero.
        /// In order to show or hide information the items between {} mean the following: {0} the
        /// start position of the records depending on page number and number of requested records;
        /// {1} - the end position {2} - total records returned from the data (default defined in language file)
        /// </summary>
        /// <param name="recordText">Record Text</param>
        public Grid SetRecordText(string recordText)
        {
            this.recordText = recordText;
            return this;
        }

        /// <summary>
        /// Defines the type of request to make (“POST” or “GET”) (default: GET)
        /// </summary>
        /// <param name="requestType">Request type</param>
        public Grid SetRequestType(RequestType requestType)
        {
            this.requestType = requestType;
            return this;
        }

        /// <summary>
        /// Assigns a class to columns that are resizable so that we can show a resize
        /// handle (default: empty string)
        /// </summary>
        /// <param name="resizeClass"></param>
        /// <returns></returns>
        public Grid SetResizeClass(string resizeClass)
        {
            this.resizeClass = resizeClass;
            return this;
        }

        /// <summary>
        /// An array to construct a select box element in the pager in which we can change the number
        /// of the visible rows. When changed during the execution, this parameter replaces the rowNum
        /// parameter that is passed to the url. If the array is empty the element does not appear
        /// in the pager. Typical you can set this like [10,20,30]. If the rowNum parameter is set to
        /// 30 then the selected value in the select box is 30.
        /// </summary>
        /// <example>
        /// setRowList(new int[]{10,20,50})
        /// </example>
        /// <param name="rowList">List of rows per page</param>
        public Grid SetRowList(int[] rowList)
        {
            this.rowList = rowList;
            return this;
        }

        /// <summary>
        /// Sets how many records we want to view in the grid. This parameter is passed to the url
        /// for use by the server routine retrieving the data. Note that if you set this parameter
        /// to 10 (i.e. retrieve 10 records) and your server return 15 then only 10 records will be
        /// loaded. Set this parameter to -1 (unlimited) to disable this checking. (default: 20)
        /// </summary>
        /// <param name="rowNum">Number of rows per page</param>
        public Grid SetRowNum(int rowNum)
        {
            this.rowNum = rowNum;
            return this;
        }

        /// <summary>
        /// If this option is set to true, a new column at the leftside of the grid is added. The purpose of
        /// this column is to count the number of available rows, beginning from 1. In this case
        /// colModel is extended automatically with a new element with name - 'rn'. Also, be careful
        /// not to use the name 'rn' in colModel
        /// </summary>
        /// <param name="rowNumbers">Boolean indicating if rownumbers are enabled</param>
        public Grid SetRowNumbers(bool rowNumbers)
        {
            this.rowNumbers = rowNumbers;
            return this;
        }

        /// <summary>
        /// Determines the width of the row number column if rownumbers option is set to true. (default: 25)
        /// </summary>
        /// <param name="rowNumWidth">Width of rownumbers column</param>
        public Grid SetRowNumWidth(int rowNumWidth)
        {
            this.rowNumWidth = rowNumWidth;
            return this;
        }

        /// <summary>
        /// Creates dynamic scrolling grids. When enabled, the pager elements are disabled and we can use the
        /// vertical scrollbar to load data. When set to true the grid will always hold all the items from the
        /// start through to the latest point ever visited.
        /// When scroll is set to an integer value (eg 1), the grid will just hold the visible lines. This allow us to
        /// load the data at portions whitout to care about the memory leaks. (default: false)
        /// </summary>
        /// <param name="scroll">Boolean indicating if scroll is enforced</param>
        public Grid SetScroll(bool scroll)
        {
            this.scroll = scroll;
            if (this.scrollInt.HasValue)
            {
                throw new InvalidOperationException("You can't set scroll to both a boolean and an integer at the same time, please choose one.");
            }

            return this;
        }

        /// <summary>
        /// Creates dynamic scrolling grids. When enabled, the pager elements are disabled and we can use the
        /// vertical scrollbar to load data. When set to true the grid will always hold all the items from the
        /// start through to the latest point ever visited.
        /// When scroll is set to an integer value (eg 1), the grid will just hold the visible lines. This allow us to
        /// load the data at portions whitout to care about the memory leaks. (default: false)
        /// </summary>
        /// <param name="scroll">When integer value is set (eg 1) scroll is enforced</param>
        public Grid SetScroll(int scroll)
        {
            this.scrollInt = scroll;
            if (this.scroll.HasValue)
            {
                throw new InvalidOperationException("You can't set scroll to both a boolean and an integer at the same time, please choose one.");
            }

            return this;
        }

        /// <summary>
        /// Determines the width of the vertical scrollbar. Since different browsers interpret this width
        /// differently (and it is difficult to calculate it in all browsers) this can be changed. (default: 18)
        /// </summary>
        /// <param name="scrollOffset">Scroll offset</param>
        public Grid SetScrollOffset(int scrollOffset)
        {
            this.scrollOffset = scrollOffset;
            return this;
        }

        /// <summary>
        /// When enabled, selecting a row with setSelection scrolls the grid so that the selected row is visible.
        /// This is especially useful when we have a verticall scrolling grid and we use form editing with
        /// navigation buttons (next or previous row). On navigating to a hidden row, the grid scrolls so the
        /// selected row becomes visible. (default: false)
        /// </summary>
        /// <param name="scrollRows">Boolean indicating if scrollrows is enabled</param>
        public Grid SetScrollRows(bool scrollRows)
        {
            this.scrollRows = scrollRows;
            return this;
        }

        /// <summary>
        /// This controls the timeout handler when scroll is set to 1. (default: 200 milliseconds)
        /// </summary>
        /// <param name="scrollTimeout">Scroll timeout in milliseconds</param>
        /// <returns></returns>
        public Grid SetScrollTimeout(int scrollTimeout)
        {
            this.scrollTimeout = scrollTimeout;
            return this;
        }

        /// <summary>
        /// When set to true adds search button to the grid. (default: false)
        /// </summary>
        /// <param name="searchToggleButton">Indicates if toggle button is displayed</param>
        public Grid SetSearch(bool search)
        {
            this.search = search;
            return this;
        }

        /// <summary>
        /// When set to true adds clear button to clear all search entries (default: false)
        /// </summary>
        /// <param name="searchClearButton"></param>
        /// <returns></returns>
        public Grid SetSearchClearButton(bool searchClearButton)
        {
            this.searchClearButton = searchClearButton;
            return this;
        }

        /// <summary>
        /// Determines how the search should be applied. If this option is set to true search is started when
        /// the user hits the enter key. If the option is false then the search is performed immediately after
        /// the user presses some character. (default: true
        /// </summary>
        /// <param name="searchOnEnter">Indicates if search is started on enter</param>
        public Grid SetSearchOnEnter(bool searchOnEnter)
        {
            this.searchOnEnter = searchOnEnter;
            return this;
        }

        /// <summary>
        /// When set to true adds toggle button to toggle search toolbar (default: false)
        /// </summary>
        /// <param name="searchToggleButton">Indicates if toggle button is displayed</param>
        public Grid SetSearchToggleButton(bool searchToggleButton)
        {
            this.searchToggleButton = searchToggleButton;
            return this;
        }

        /// <summary>
        /// Enables toolbar searching / filtering
        /// </summary>
        /// <param name="searchToolbar">Indicates if toolbar searching is enabled</param>
        public Grid SetSearchToolbar(bool searchToolbar)
        {
            this.searchToolbar = searchToolbar;
            return this;
        }

        /// <summary>
        /// If enabled all sort icons are visible for all columns which are sortable (default: false)
        /// </summary>
        /// <param name="showAllSortIcons">Boolean indicating if all sorting icons should be displayed</param>
        public Grid SetShowAllSortIcons(bool showAllSortIcons)
        {
            this.showAllSortIcons = showAllSortIcons;
            return this;
        }

        /// <summary>
        /// This option describes the type of calculation of the initial width of each column
        /// against the width of the grid. If the value is true and the value in width option
        /// is set then: Every column width is scaled according to the defined option width.
        /// Example: if we define two columns with a width of 80 and 120 pixels, but want the
        /// grid to have a 300 pixels - then the columns are recalculated as follow:
        /// 1- column = 300(new width)/200(sum of all width)*80(column width) = 120 and 2 column = 300/200*120 = 180.
        /// The grid width is 300px. If the value is false and the value in width option is set then:
        /// The width of the grid is the width set in option.
        /// The column width are not recalculated and have the values defined in colModel. (default: true)
        /// </summary>
        /// <param name="shrinkToFit">Boolean indicating if shrink to fit is enforced</param>
        public Grid SetShrinkToFit(bool shrinkToFit)
        {
            this.shrinkToFit = shrinkToFit;
            return this;
        }

        /// <summary>
        /// Sets direction in which sort icons are displayed (default: vertical)
        /// </summary>
        /// <param name="sortIconDirection">Direction in which sort icons are displayed</param>
        public Grid SetSortIconDirection(Direction sortIconDirection)
        {
            this.sortIconDirection = sortIconDirection;
            return this;
        }

        /// <summary>
        /// The initial sorting name when we use datatypes xml or json (data returned from server).
        /// This parameter is added to the url. If set and the index (name) matches the name from the
        /// colModel then by default an image sorting icon is added to the column, according to
        /// the parameter sortorder.
        /// </summary>
        /// <param name="sortName"></param>
        public Grid SetSortName(string sortName)
        {
            this.sortName = sortName;
            return this;
        }

        /// <summary>
        /// If enabled columns are sorted when header is clicked (default: true)
        /// Warning, if disabled and setShowAllSortIcons is set to false, sorting will
        /// be effectively disabled
        /// </summary>
        /// <param name="sortOnHeaderClick">Boolean indicating if columns will sort on headerclick</param>
        /// <returns></returns>
        public Grid SetSortOnHeaderClick(bool sortOnHeaderClick)
        {
            this.sortOnHeaderClick = sortOnHeaderClick;
            return this;
        }

        /// <summary>
        /// The initial sorting order when we use datatypes xml or json (data returned from server).
        /// This parameter is added to the url. Two possible values - asc or desc. (default: asc)
        /// </summary>
        /// <param name="sortOrder">Sortorder</param>
        public Grid SetSortOrder(SortOrder sortOrder)
        {
            this.sortOrder = sortOrder;
            return this;
        }

        /// <summary>
        /// This option enabled the toolbar of the grid.  When we have two toolbars (can be set using setToolbarPosition)
        /// then two elements (div) are automatically created. The id of the top bar is constructed like
        /// “t_”+id of the grid and the bottom toolbar the id is “tb_”+id of the grid. In case when
        /// only one toolbar is created we have the id as “t_” + id of the grid, independent of where
        /// this toolbar is created (top or bottom). You can use jquery to add elements to the toolbars.
        /// </summary>
        /// <param name="toolbar">Boolean indicating if toolbar is enabled</param>
        public Grid SetToolbar(bool toolbar)
        {
            this.toolbar = toolbar;
            return this;
        }

        /// <summary>
        /// Sets toolbarposition (default: top)
        /// </summary>
        /// <param name="toolbarPosition">Position of toolbar</param>
        public Grid SetToolbarPosition(ToolbarPosition toolbarPosition)
        {
            this.toolbarPosition = toolbarPosition;
            return this;
        }

        /// <summary>
        /// When enabled this option place a pager element at top of the grid below the caption
        /// (if available). If another pager is defined both can coexists and are refreshed in sync.
        /// (default: false)
        /// </summary>
        /// <param name="topPager">Boolean indicating if toppager is enabled</param>
        public Grid SetTopPager(bool topPager)
        {
            this.topPager = topPager;
            return this;
        }

        /// <summary>
        /// The url of the file that holds the request
        /// </summary>
        /// <param name="url">Data url</param>
        public Grid SetUrl(string url)
        {
            this.url = url;
            return this;
        }

        /// <summary>
        /// If true, jqGrid displays the beginning and ending record number in the grid,
        /// out of the total number of records in the query.
        /// This information is shown in the pager bar (bottom right by default)in this format:
        /// “View X to Y out of Z”.
        /// If this value is true, there are other parameters that can be adjusted,
        /// including 'emptyrecords' and 'recordtext'. (default: false)
        /// </summary>
        /// <param name="viewRecords">Boolean indicating if recordnumbers are shown in grid</param>
        public Grid SetViewRecords(bool viewRecords)
        {
            this.viewRecords = viewRecords;
            return this;
        }

        /// <summary>
        /// If this option is not set, the width of the grid is a sum of the widths of the columns
        /// defined in the colModel (in pixels). If this option is set, the initial width of each
        /// column is set according to the value of shrinkToFit option.
        /// </summary>
        /// <param name="width">Width in pixels</param>
        public Grid SetWidth(int width)
        {
            this.width = width;
            return this;
        }

        /// <summary>
        /// Creates and returns javascript + required html elements to render grid
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // Create javascript
            StringBuilder script = new StringBuilder();

            // Start script
            script.AppendLine("<script type=\"text/javascript\">");
            script.AppendLine("jQuery(document).ready(function () {");
            script.AppendLine("jQuery('#" + this.id + "').jqGrid({");

            // Altrows
            if (this.altRows.HasValue)
            {
                script.AppendFormat("altRows: {0},", this.altRows.Value.ToString().ToLower()).AppendLine();
            }

            // Altclass
            if (!string.IsNullOrWhiteSpace(this.altClass))
            {
                script.AppendFormat("altclass: '{0}',", this.altClass).AppendLine();
            }

            // Autoencode
            if (this.autoEncode.HasValue)
            {
                script.AppendFormat("autoencode: {0},", this.autoEncode.Value.ToString().ToLower()).AppendLine();
            }

            // Autowidth
            if (this.autoWidth.HasValue)
            {
                script.AppendFormat("autowidth: {0},", this.autoWidth.Value.ToString().ToLower()).AppendLine();
            }

            // Caption
            if (!string.IsNullOrWhiteSpace(this.caption))
            {
                script.AppendFormat("caption: '{0}',", this.caption).AppendLine();
            }

            // Datatype
            script.AppendLine(string.Format("datatype:'{0}',", this.dataType.ToString()));

            if (this.dataType == DataType.local)
            {
                script.AppendLine(string.Format("data: {0},", this.data));
            }

            // Emptyrecords
            if (!string.IsNullOrWhiteSpace(this.emptyRecords))
            {
                script.AppendFormat("emptyrecords: '{0}',", this.emptyRecords).AppendLine();
            }

            // FooterRow
            if (this.footerRow.HasValue)
            {
                script.AppendFormat("footerrow: {0},", this.footerRow.Value.ToString().ToLower()).AppendLine();
            }

            // Forcefit
            if (this.forceFit.HasValue)
            {
                script.AppendFormat("forceFit: {0},", this.forceFit.Value.ToString().ToLower()).AppendLine();
            }

            // Gridview
            if (this.gridView.HasValue)
            {
                script.AppendFormat("gridview: {0},", this.gridView.Value.ToString().ToLower()).AppendLine();
            }

            // HeaderTitles
            if (this.headerTitles.HasValue)
            {
                script.AppendFormat("headertitles: {0},", this.headerTitles.Value.ToString().ToLower()).AppendLine();
            }

            // Height (set 100% if no value is specified except when scroll is set to true otherwise layout is not as it is supposed to be)
            if (!this.height.HasValue)
            {
                if ((!this.scroll.HasValue || this.scroll.Value == false) && !this.scrollInt.HasValue)
                {
                    script.AppendFormat("height: '{0}',", "100%").AppendLine();
                }
            }
            else
            {
                script.AppendFormat("height: {0},", this.height).AppendLine();
            }

            // Hiddengrid
            if (this.hiddenGrid.HasValue)
            {
                script.AppendFormat("hiddengrid: {0},", this.hiddenGrid.Value.ToString().ToLower()).AppendLine();
            }

            // Hidegrid
            if (this.hideGrid.HasValue)
            {
                script.AppendFormat("hidegrid: {0},", this.hideGrid.Value.ToString().ToLower()).AppendLine();
            }

            // HoverRows
            if (this.hoverRows.HasValue)
            {
                script.AppendFormat("hoverrows: {0},", this.hoverRows.Value.ToString().ToLower()).AppendLine();
            }

            // Loadonce
            if (this.loadOnce.HasValue)
            {
                script.AppendFormat("loadonce: {0},", this.loadOnce.Value.ToString().ToLower()).AppendLine();
            }

            // Loadtext
            if (!string.IsNullOrWhiteSpace(this.loadText))
            {
                script.AppendFormat("loadtext: '{0}',", this.loadText).AppendLine();
            }

            // LoadUi
            if (this.loadUI.HasValue)
            {
                script.AppendFormat("loadui: '{0}',", this.loadUI.Value.ToString()).AppendLine();
            }

            // MultiBoxOnly
            if (this.multiBoxOnly.HasValue)
            {
                script.AppendFormat("multiboxonly: {0},", this.multiBoxOnly.Value.ToString().ToLower()).AppendLine();
            }

            // MultiKey
            if (this.multiKey.HasValue)
            {
                script.AppendFormat("multikey: '{0}',", this.multiKey.Value.ToString()).AppendLine();
            }

            // MultiSelect
            if (this.multiSelect.HasValue)
            {
                script.AppendFormat("multiselect: {0},", this.multiSelect.Value.ToString().ToLower()).AppendLine();
            }

            // MultiSelectWidth
            if (this.multiSelectWidth.HasValue)
            {
                script.AppendFormat("multiselectWidth: {0},", this.multiSelectWidth.Value.ToString()).AppendLine();
            }

            // Page
            if (this.page.HasValue)
            {
                script.AppendFormat("page:{0},", this.page.Value).AppendLine();
            }

            // Pager
            if (!string.IsNullOrWhiteSpace(this.pager))
            {
                script.AppendFormat("pager:'#{0}',", this.pager).AppendLine();
            }

            // PagerPos
            if (this.pagerPos.HasValue)
            {
                script.AppendFormat("pagerpos: '{0}',", this.pagerPos.ToString()).AppendLine();
            }

            // PgButtons
            if (this.pgButtons.HasValue)
            {
                script.AppendFormat("pgbuttons:{0},", this.pgButtons.Value.ToString().ToLower()).AppendLine();
            }

            // PgInput
            if (this.pgInput.HasValue)
            {
                script.AppendFormat("pginput: {0},", this.pgInput.Value.ToString().ToLower()).AppendLine();
            }

            // PGText
            if (!string.IsNullOrWhiteSpace(this.pgText))
            {
                script.AppendFormat("pgtext: '{0}',", this.pgText).AppendLine();
            }

            // RecordPos
            if (this.recordPos.HasValue)
            {
                script.AppendFormat("recordpos: '{0}',", this.recordPos.Value).AppendLine();
            }

            // RecordText
            if (!string.IsNullOrWhiteSpace(this.recordText))
            {
                script.AppendFormat("recordtext: '{0}',", this.recordText).AppendLine();
            }

            // Request Type
            if (this.requestType.HasValue)
            {
                script.AppendFormat("mtype: '{0}',", this.requestType.ToString()).AppendLine();
            }

            // ResizeClass
            if (!string.IsNullOrWhiteSpace(this.resizeClass))
            {
                script.AppendFormat("resizeclass: '{0}',", this.resizeClass).AppendLine();
            }

            // Rowlist
            if (this.rowList != null)
            {
                script.AppendFormat("rowList: [{0}],", string.Join(",", (from p in this.rowList select p.ToString()).ToArray())).AppendLine();
            }

            // Rownum
            if (this.rowNum.HasValue)
            {
                script.AppendFormat("rowNum:{0},", this.rowNum.Value).AppendLine();
            }

            // Rownumbers
            if (this.rowNumbers.HasValue)
            {
                script.AppendFormat("rownumbers: {0},", this.rowNumbers.Value.ToString().ToLower()).AppendLine();
            }

            // RowNumWidth
            if (this.rowNumWidth.HasValue)
            {
                script.AppendFormat("rownumWidth: {0},", this.rowNumWidth.Value.ToString()).AppendLine();
            }

            // Scroll (setters make sure either scroll or scrollint is set, never both)
            if (this.scroll.HasValue)
            {
                script.AppendFormat("scroll:{0},", this.scroll.ToString().ToLower()).AppendLine();
            }

            if (this.scrollInt.HasValue)
            {
                script.AppendFormat("scroll:{0},", this.scrollInt.Value).AppendLine();
            }

            // ScrollOffset
            if (this.scrollOffset.HasValue)
            {
                script.AppendFormat("scrollOffset: {0},", this.scrollOffset.Value).AppendLine();
            }

            // ScrollRows
            if (this.scrollRows.HasValue)
            {
                script.AppendFormat("scrollrows: {0},", this.scrollRows.ToString().ToLower()).AppendLine();
            }

            // ScrollTimeout
            if (this.scrollTimeout.HasValue)
            {
                script.AppendFormat("scrollTimeout: {0},", this.scrollTimeout.Value).AppendLine();
            }

            // Sortname
            if (!string.IsNullOrWhiteSpace(this.sortName))
            {
                script.AppendFormat("sortname: '{0}',", this.sortName).AppendLine();
            }

            // Sorticons
            if (this.showAllSortIcons.HasValue || this.sortIconDirection.HasValue || this.sortOnHeaderClick.HasValue)
            {
                // Set defaults
                if (!this.showAllSortIcons.HasValue)
                {
                    this.showAllSortIcons = false;
                }

                if (!this.sortIconDirection.HasValue)
                {
                    this.sortIconDirection = Direction.vertical;
                }

                if (!this.sortOnHeaderClick.HasValue)
                {
                    this.sortOnHeaderClick = true;
                }

                script.AppendFormat("viewsortcols: [{0},'{1}',{2}],", this.showAllSortIcons.Value.ToString().ToLower(), this.sortIconDirection.ToString(), this.sortOnHeaderClick.Value.ToString().ToLower()).AppendLine();
            }

            // Shrink to fit
            if (this.shrinkToFit.HasValue)
            {
                script.AppendFormat("shrinkToFit: {0},", this.shrinkToFit.Value.ToString().ToLower()).AppendLine();
            }

            // Sortorder
            if (this.sortOrder.HasValue)
            {
                script.AppendFormat("sortorder: '{0}',", this.sortOrder.Value.ToString()).AppendLine();
            }

            // Toolbar
            if (this.toolbar.HasValue)
            {
                script.AppendFormat("toolbar: [{0},\"{1}\"],", this.toolbar.Value.ToString().ToLower(), this.toolbarPosition.ToString()).AppendLine();
            }

            // Toppager
            if (this.topPager.HasValue)
            {
                script.AppendFormat("toppager: {0},", this.topPager.Value.ToString().ToLower()).AppendLine();
            }

            // Url
            if (!string.IsNullOrWhiteSpace(this.url))
            {
                script.AppendFormat("url:'{0}',", this.url).AppendLine();
            }

            // View records
            if (this.viewRecords.HasValue)
            {
                script.AppendFormat("viewrecords: {0},", this.viewRecords.Value.ToString().ToLower()).AppendLine();
            }

            // Width
            if (this.width.HasValue)
            {
                script.AppendFormat("width:'{0}',", this.width.Value).AppendLine();
            }

            // onAfterInsertRow
            if (!string.IsNullOrWhiteSpace(this.onAfterInsertRow))
            {
                script.AppendFormat("afterInsertRow: function(rowid, rowdata, rowelem) {{{0}}},", this.onAfterInsertRow).AppendLine();
            }

            // onBeforeRequest
            if (!string.IsNullOrWhiteSpace(this.onBeforeRequest))
            {
                script.AppendFormat("beforeRequest: function() {{{0}}},", this.onBeforeRequest).AppendLine();
            }

            // onBeforeSelectRow
            if (!string.IsNullOrWhiteSpace(this.onBeforeSelectRow))
            {
                script.AppendFormat("beforeSelectRow: function(rowid, e) {{{0}}},", this.onBeforeSelectRow).AppendLine();
            }

            // onGridComplete
            if (!string.IsNullOrWhiteSpace(this.onGridComplete))
            {
                script.AppendFormat("gridComplete: function() {{{0}}},", this.onGridComplete).AppendLine();
            }

            // onLoadBeforeSend
            if (!string.IsNullOrWhiteSpace(this.onLoadBeforeSend))
            {
                script.AppendFormat("loadBeforeSend: function(xhr) {{{0}}},", this.onLoadBeforeSend).AppendLine();
            }

            // onLoadComplete
            if (!string.IsNullOrWhiteSpace(this.onLoadComplete))
            {
                script.AppendFormat("loadComplete: function(xhr) {{{0}}},", this.onLoadComplete).AppendLine();
            }

            // onLoadError
            if (!string.IsNullOrWhiteSpace(this.onLoadError))
            {
                script.AppendFormat("loadError: function(xhr, status, error) {{{0}}},", this.onLoadError).AppendLine();
            }

            // onCellSelect
            if (!string.IsNullOrWhiteSpace(this.onCellSelect))
            {
                script.AppendFormat("onCellSelect: function(rowid, iCol, cellcontent, e) {{{0}}},", this.onCellSelect).AppendLine();
            }

            // onDblClickRow
            if (!string.IsNullOrWhiteSpace(this.onDblClickRow))
            {
                script.AppendFormat("ondblClickRow: function(rowid, iRow, iCol, e) {{{0}}},", this.onDblClickRow).AppendLine();
            }

            // onHeaderClick
            if (!string.IsNullOrWhiteSpace(this.onHeaderClick))
            {
                script.AppendFormat("onHeaderClick: function(gridstate) {{{0}}},", this.onHeaderClick).AppendLine();
            }

            // onPaging
            if (!string.IsNullOrWhiteSpace(this.onPaging))
            {
                script.AppendFormat("onPaging: function(pgButton) {{{0}}},", this.onPaging).AppendLine();
            }

            // onRightClickRow
            if (!string.IsNullOrWhiteSpace(this.onRightClickRow))
            {
                script.AppendFormat("onRightClickRow: function(rowid, iRow, iCol, e) {{{0}}},", this.onRightClickRow).AppendLine();
            }

            // onSelectAll
            if (!string.IsNullOrWhiteSpace(this.onSelectAll))
            {
                script.AppendFormat("onSelectAll: function(aRowids, status) {{{0}}},", this.onSelectAll).AppendLine();
            }

            // onSelectRow event
            if (!string.IsNullOrWhiteSpace(this.onSelectRow))
            {
                script.AppendFormat("onSelectRow: function(rowid, status) {{{0}}},", this.onSelectRow).AppendLine();
            }

            // onSortCol
            if (!string.IsNullOrWhiteSpace(this.onSortCol))
            {
                script.AppendFormat("onSortCol: function(index, iCol, sortorder) {{{0}}},", this.onSortCol).AppendLine();
            }

            // onResizeStart
            if (!string.IsNullOrWhiteSpace(this.onResizeStart))
            {
                script.AppendFormat("resizeStart: function(event, index) {{{0}}},", this.onResizeStart).AppendLine();
            }

            // onResizeStop
            if (!string.IsNullOrWhiteSpace(this.onResizeStop))
            {
                script.AppendFormat("resizeStop: function(newwidth, index) {{{0}}},", this.onResizeStop).AppendLine();
            }

            // onSerializeGridData
            if (!string.IsNullOrWhiteSpace(this.onSerializeGridData))
            {
                script.AppendFormat("serializeGridData: function(postData) {{{0}}},", this.onSerializeGridData).AppendLine();
            }

            // Colmodel
            script.AppendLine("colModel: [");
            string colModel = string.Join(",", (from c in this.columns select c.ToString()).ToArray());
            script.AppendLine(colModel);
            script.AppendLine("]");

            if (this.grouping)
            {
                script.Append(",");
                script.AppendLine("grouping: true,");
                script.AppendLine("groupingView: {");
                script.AppendLine(string.Format(CultureInfo.InvariantCulture, "groupField: ['{0}'],", string.Join("', '", this.groupFields.ToArray())));
                
                if (this.groupColumnShow.Any())
                {
                    script.AppendLine(string.Format(CultureInfo.InvariantCulture, "groupColumnShow: [{0}],", string.Join(", ", this.groupColumnShow.Select(g => g.ToString().ToLower()).ToArray())));
                }

                if (!string.IsNullOrEmpty(this.groupText))
                {
                    script.AppendLine(string.Format(CultureInfo.InvariantCulture, "groupText: {0},", this.groupText));
                }

                if (this.groupCollapse)
                {
                    script.AppendLine("groupCollapse: true,");
                }

                if (this.groupOrder.Any())
                {
                    script.AppendLine(string.Format(CultureInfo.InvariantCulture, "groupOrder: ['{0}'],", string.Join("', '", this.groupOrder.ToArray())));
                }

                if (this.groupSummary.Any())
                {
                    script.AppendLine(string.Format(CultureInfo.InvariantCulture, "groupSummary: [{0}],", string.Join(", ", this.groupSummary.Select(g => g.ToString().ToLower()).ToArray())));
                }
                
                script.AppendLine("}");
            }

            // End jqGrid call
            script.AppendLine("});");

            // Search clear button
            if (this.searchToolbar == true && this.searchClearButton.HasValue && !string.IsNullOrWhiteSpace(this.pager) && this.searchClearButton.Value == true)
            {
                script.AppendLine("jQuery('#" + this.id + "').jqGrid('navButtonAdd',\"#" +
                                  this.pager + "\",{caption:\"Clear\",title:\"Clear Search\",buttonicon :'ui-icon-refresh', onClickButton:function(){mygrid[0].clearToolbar(); }}); ");
            }

            // Search toolbar
            if (this.searchToolbar == true)
            {
                script.Append("jQuery('#" + this.id + "').jqGrid('filterToolbar', {stringResult:true");
                if (this.searchOnEnter.HasValue)
                {
                    script.AppendFormat(", searchOnEnter:{0}", this.searchOnEnter.Value.ToString().ToLower());
                }

                script.AppendLine("});");
            }

            var searhBoxOptions = string.Format(
                                      "closeOnEscape: {0}, multipleSearch: {1}, closeAfterSearch: {2} ",
                                      this.closeOnEscape.ToString().ToLower(),
                                      this.multipleSearch.ToString().ToLower(),
                                      this.closeAfterSearch.ToString().ToLower());

            // CFM
            script.AppendLine("jQuery('#" + this.id + "').jqGrid('navGrid',\"#"
                + this.pager + "\", { edit: "
                + this.edit.ToString().ToLower() + ", add: "
                + this.add.ToString().ToLower() + ", del: "
                + this.delete.ToString().ToLower() + ", search:"
                + this.search.ToString().ToLower() + ", refresh: true }, {}, {},{}, {"
                + searhBoxOptions + "}, {}); ");

            // End script
            script.AppendLine("});");
            script.AppendLine("</script>");

            // Create table which is used to render grid
            var table = new StringBuilder();
            table.AppendFormat("<table id=\"{0}\"></table>", this.id);

            // Create pager element if is set
            var pager = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(this.pager))
            {
                pager.AppendFormat("<div id=\"{0}\"></div>", this.pager);
            }

            // Create toppager element if is set
            var topPager = new StringBuilder();
            if (this.topPager == true)
            {
                topPager.AppendFormat("<div id=\"{0}_toppager\"></div>", this.id);
            }

            // Insert grid id where needed (in columns)
            script.Replace("##gridid##", this.id);

            // Return script + required elements
            return script.ToString() + table.ToString() + pager.ToString() + topPager.ToString();
        }
    }

    /// <summary>
    /// jqGrid HtmlHelper extension methods.
    /// </summary>
    public static class GridHelper
    {
        /// <summary>
        /// Extends HtmlHelper so we can crerate jqGrids.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="id">The id of the grid</param>
        /// <returns></returns>
        public static Grid Grid(this System.Web.Mvc.HtmlHelper helper, string id)
        {
            return new Grid(id);
        }
    }
}