﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeta.WisdCar.Model.VO
{
    public class KVLookupVO
    {
        #region Custom
        private string _rowid;
        private string _operation;
        /// <summary>
        /// Datatables Row ID
        /// </summary>
        public string DT_RowId
        {
            set { _rowid = value; }
            get { return _rowid; }
        }
        /// <summary>
        /// 操作
        /// </summary>
        public string Operation
        {
            set { _operation = value; }
            get { return _operation; }
        }
        #endregion


        #region Model
        private int _lookupid;
        private string _lookupkey;
        private string _lookupvalue;
        private int _categoryid;
        private int _logicalstatus = 1;
        private string _creatorid;
        private DateTime _createddate;
        private string _lastmodifierid;
        private DateTime _lastmodifieddate;
        private string _reserved1;
        private string _reserved2;
        private string _reserved3;
        /// <summary>
        /// 数据字典表主键
        /// </summary>
        public int LookupID
        {
            set { _lookupid = value; }
            get { return _lookupid; }
        }
        /// <summary>
        /// 数据字典Key
        /// </summary>
        public string LookupKey
        {
            set { _lookupkey = value; }
            get { return _lookupkey; }
        }
        /// <summary>
        /// 数据字典Value
        /// </summary>
        public string LookupValue
        {
            set { _lookupvalue = value; }
            get { return _lookupvalue; }
        }
        /// <summary>
        /// 数据类型表主键
        /// </summary>
        public int CategoryID
        {
            set { _categoryid = value; }
            get { return _categoryid; }
        }
        /// <summary>
        /// 逻辑状态
        /// </summary>
        public int LogicalStatus
        {
            set { _logicalstatus = value; }
            get { return _logicalstatus; }
        }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public string CreatorID
        {
            set { _creatorid = value; }
            get { return _creatorid; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate
        {
            set { _createddate = value; }
            get { return _createddate; }
        }
        /// <summary>
        /// 最近修改人ID
        /// </summary>
        public string LastModifierID
        {
            set { _lastmodifierid = value; }
            get { return _lastmodifierid; }
        }
        /// <summary>
        /// 最近修改时间
        /// </summary>
        public DateTime LastModifiedDate
        {
            set { _lastmodifieddate = value; }
            get { return _lastmodifieddate; }
        }
        /// <summary>
        /// 保留字段1
        /// </summary>
        public string Reserved1
        {
            set { _reserved1 = value; }
            get { return _reserved1; }
        }
        /// <summary>
        /// 保留字段2
        /// </summary>
        public string Reserved2
        {
            set { _reserved2 = value; }
            get { return _reserved2; }
        }
        /// <summary>
        /// 保留字段3
        /// </summary>
        public string Reserved3
        {
            set { _reserved3 = value; }
            get { return _reserved3; }
        }
        #endregion Model
    }
}
