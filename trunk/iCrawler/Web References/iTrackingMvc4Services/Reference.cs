﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.17929.
// 
#pragma warning disable 1591

namespace iCrawler.iTrackingMvc4Services {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="iTrackingServicesSoap", Namespace="http://tempuri.org/")]
    public partial class iTrackingServices : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetArticleOperationCompleted;
        
        private System.Threading.SendOrPostCallback CreateArticleOperationCompleted;
        
        private System.Threading.SendOrPostCallback HelloWorldOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public iTrackingServices() {
            this.Url = global::iCrawler.Properties.Settings.Default.iCrawler_iTrackingMvc4Services_iTrackingServices;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetArticleCompletedEventHandler GetArticleCompleted;
        
        /// <remarks/>
        public event CreateArticleCompletedEventHandler CreateArticleCompleted;
        
        /// <remarks/>
        public event HelloWorldCompletedEventHandler HelloWorldCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetArticle", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public Article GetArticle(System.Guid articleId) {
            object[] results = this.Invoke("GetArticle", new object[] {
                        articleId});
            return ((Article)(results[0]));
        }
        
        /// <remarks/>
        public void GetArticleAsync(System.Guid articleId) {
            this.GetArticleAsync(articleId, null);
        }
        
        /// <remarks/>
        public void GetArticleAsync(System.Guid articleId, object userState) {
            if ((this.GetArticleOperationCompleted == null)) {
                this.GetArticleOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetArticleOperationCompleted);
            }
            this.InvokeAsync("GetArticle", new object[] {
                        articleId}, this.GetArticleOperationCompleted, userState);
        }
        
        private void OnGetArticleOperationCompleted(object arg) {
            if ((this.GetArticleCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetArticleCompleted(this, new GetArticleCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CreateArticle", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public Article CreateArticle(string title, string summary, string content, string author) {
            object[] results = this.Invoke("CreateArticle", new object[] {
                        title,
                        summary,
                        content,
                        author});
            return ((Article)(results[0]));
        }
        
        /// <remarks/>
        public void CreateArticleAsync(string title, string summary, string content, string author) {
            this.CreateArticleAsync(title, summary, content, author, null);
        }
        
        /// <remarks/>
        public void CreateArticleAsync(string title, string summary, string content, string author, object userState) {
            if ((this.CreateArticleOperationCompleted == null)) {
                this.CreateArticleOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateArticleOperationCompleted);
            }
            this.InvokeAsync("CreateArticle", new object[] {
                        title,
                        summary,
                        content,
                        author}, this.CreateArticleOperationCompleted, userState);
        }
        
        private void OnCreateArticleOperationCompleted(object arg) {
            if ((this.CreateArticleCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CreateArticleCompleted(this, new CreateArticleCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/HelloWorld", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string HelloWorld() {
            object[] results = this.Invoke("HelloWorld", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void HelloWorldAsync() {
            this.HelloWorldAsync(null);
        }
        
        /// <remarks/>
        public void HelloWorldAsync(object userState) {
            if ((this.HelloWorldOperationCompleted == null)) {
                this.HelloWorldOperationCompleted = new System.Threading.SendOrPostCallback(this.OnHelloWorldOperationCompleted);
            }
            this.InvokeAsync("HelloWorld", new object[0], this.HelloWorldOperationCompleted, userState);
        }
        
        private void OnHelloWorldOperationCompleted(object arg) {
            if ((this.HelloWorldCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class Article {
        
        private System.Guid idField;
        
        private string titleField;
        
        private string summaryField;
        
        private string contentField;
        
        private System.Nullable<System.DateTime> createDateField;
        
        private string createByField;
        
        private System.Nullable<System.DateTime> lastUpdateField;
        
        private string updateByField;
        
        private System.Nullable<System.Guid> categoryIdField;
        
        private System.Nullable<int> countViewsField;
        
        private System.Nullable<bool> isSentField;
        
        private System.Nullable<System.DateTime> sentDateField;
        
        private System.Nullable<bool> isReviewedField;
        
        private System.Nullable<System.DateTime> reviewedDateField;
        
        private System.Nullable<bool> isApprovedField;
        
        private System.Nullable<System.DateTime> approvedDateField;
        
        private System.Nullable<bool> isPublishedField;
        
        private System.Nullable<System.DateTime> publishedDateField;
        
        private System.Nullable<bool> isReturnedField;
        
        private System.Nullable<System.DateTime> returnedDateField;
        
        private string tagsField;
        
        private string approvedByField;
        
        private string avatarPathField;
        
        /// <remarks/>
        public System.Guid Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        public string Title {
            get {
                return this.titleField;
            }
            set {
                this.titleField = value;
            }
        }
        
        /// <remarks/>
        public string Summary {
            get {
                return this.summaryField;
            }
            set {
                this.summaryField = value;
            }
        }
        
        /// <remarks/>
        public string Content {
            get {
                return this.contentField;
            }
            set {
                this.contentField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<System.DateTime> CreateDate {
            get {
                return this.createDateField;
            }
            set {
                this.createDateField = value;
            }
        }
        
        /// <remarks/>
        public string CreateBy {
            get {
                return this.createByField;
            }
            set {
                this.createByField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<System.DateTime> LastUpdate {
            get {
                return this.lastUpdateField;
            }
            set {
                this.lastUpdateField = value;
            }
        }
        
        /// <remarks/>
        public string UpdateBy {
            get {
                return this.updateByField;
            }
            set {
                this.updateByField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<System.Guid> CategoryId {
            get {
                return this.categoryIdField;
            }
            set {
                this.categoryIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> CountViews {
            get {
                return this.countViewsField;
            }
            set {
                this.countViewsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<bool> IsSent {
            get {
                return this.isSentField;
            }
            set {
                this.isSentField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<System.DateTime> SentDate {
            get {
                return this.sentDateField;
            }
            set {
                this.sentDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<bool> IsReviewed {
            get {
                return this.isReviewedField;
            }
            set {
                this.isReviewedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<System.DateTime> ReviewedDate {
            get {
                return this.reviewedDateField;
            }
            set {
                this.reviewedDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<bool> IsApproved {
            get {
                return this.isApprovedField;
            }
            set {
                this.isApprovedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<System.DateTime> ApprovedDate {
            get {
                return this.approvedDateField;
            }
            set {
                this.approvedDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<bool> IsPublished {
            get {
                return this.isPublishedField;
            }
            set {
                this.isPublishedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<System.DateTime> PublishedDate {
            get {
                return this.publishedDateField;
            }
            set {
                this.publishedDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<bool> IsReturned {
            get {
                return this.isReturnedField;
            }
            set {
                this.isReturnedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<System.DateTime> ReturnedDate {
            get {
                return this.returnedDateField;
            }
            set {
                this.returnedDateField = value;
            }
        }
        
        /// <remarks/>
        public string Tags {
            get {
                return this.tagsField;
            }
            set {
                this.tagsField = value;
            }
        }
        
        /// <remarks/>
        public string ApprovedBy {
            get {
                return this.approvedByField;
            }
            set {
                this.approvedByField = value;
            }
        }
        
        /// <remarks/>
        public string AvatarPath {
            get {
                return this.avatarPathField;
            }
            set {
                this.avatarPathField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetArticleCompletedEventHandler(object sender, GetArticleCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetArticleCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetArticleCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Article Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Article)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void CreateArticleCompletedEventHandler(object sender, CreateArticleCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CreateArticleCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CreateArticleCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Article Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Article)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void HelloWorldCompletedEventHandler(object sender, HelloWorldCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class HelloWorldCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal HelloWorldCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591