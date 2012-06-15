<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" indent="yes" />

  <xsl:key name="fields" match="/Entity/Fields/Field" use="@ID"/>
  <xsl:key name="alltables" match="/Model/EntityCollection/Entity" use="@ID"/>
  <xsl:key name="allfields" match="/Model/EntityCollection/Entity/Fields/Field" use="@ID"/>
  <xsl:key name="allrelations" match="/Model/Relations/Relation" use="@ID"/>

  <xsl:template match="/" >
    
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;    


    <xsl:apply-templates select="Model" /><!-- Processing the Model when in Single File Mode -->
    <xsl:apply-templates select="Entity"/><!-- Only one Entity when in MultiFileMode File Mode -->
  </xsl:template>

  <xsl:template match="Model">
    <xsl:apply-templates select="EntityCollection/Entity"/>
  </xsl:template>
  
  <xsl:template match="Entity">

    namespace <xsl:value-of select="LogicalPackage" /><xsl:text >.</xsl:text>
              <xsl:value-of select="LogicalModule" />
    {
      public class <xsl:value-of select="LogicalName" />
      {
      
        ///
        /// Default Constructor
        ///
        public <xsl:value-of select="LogicalName" />()
        {
        }
        <xsl:apply-templates select="Constraints" />
    
        #region Property Accessors
        <xsl:apply-templates select="Fields" />
        #endregion
        
        <xsl:apply-templates select="ChildRelations" />
      }
    }
  </xsl:template>

  <xsl:template match="ChildRelations">

    #region  Related table accessors 

    <xsl:apply-templates select="RelationReference" mode="child"/>

    #endregion
  </xsl:template >

  <xsl:template match="RelationReference" mode="child">
    ///
    ///  MultiFile <xsl:value-of select="@IDREF" /><xsl:value-of select="Relation/LogicalName" />
    ///  Single file <xsl:value-of select="key('allrelations', @IDREF)/LogicalName" /> generated
    ///
    <xsl:apply-templates select="Relation" mode="child"/>
    <xsl:apply-templates select="key('allrelations', @IDREF)" mode="child"/>

  </xsl:template >

  <xsl:template match="Relation" mode="child">

    <xsl:apply-templates select="ChildEntity/Entity" mode="child"/>
    <xsl:apply-templates select="key('alltables', ChildEntity/@IDREF)" mode="child"/>

  </xsl:template >
  <xsl:template match="Entity" mode="child">
    /// 
    /// <xsl:value-of select="LogicalName" />
    ///
    <xsl:choose>
    <xsl:when test="/Entity/@UseCollections = 'True'">
    private <xsl:value-of select="LogicalPackage" />.<xsl:text/>
        <xsl:value-of select="LogicalModule" />.Collections.<xsl:text/>
        <xsl:value-of select="LogicalName" />Collection _<xsl:text/>
        <xsl:value-of select="LogicalName" />;        
   public <xsl:value-of select="LogicalPackage" />.<xsl:text/>
  <xsl:value-of select="LogicalModule" />.Collections.<xsl:text/>
  <xsl:value-of select="LogicalName" />Collection <xsl:text/>       
      </xsl:when>
      <xsl:otherwise>
        private List&lt;<xsl:text/>
        <xsl:value-of select="LogicalPackage" />.<xsl:text/>
        <xsl:value-of select="LogicalModule" />.<xsl:text/>
        <xsl:value-of select="LogicalName" />&gt; _<xsl:text/>
        <xsl:value-of select="LogicalName" />;
        public List&lt;<xsl:text/>
        <xsl:value-of select="LogicalPackage" />.<xsl:text/>
        <xsl:value-of select="LogicalModule" />.<xsl:text/>
        <xsl:value-of select="LogicalName" />&gt; <xsl:text/>
      </xsl:otherwise>
    </xsl:choose> 


  <xsl:choose>
	<xsl:when test="LogicalName = ../../ParentEntity/Entity/LogicalName">
		<xsl:value-of select="LogicalName" /><xsl:text>Child</xsl:text>
	</xsl:when>
	<xsl:otherwise>
    	<xsl:value-of select="LogicalName" />
	</xsl:otherwise>
	</xsl:choose>
    {
      get
      {
        <xsl:value-of select="LogicalPackage" />.<xsl:text/>
    <xsl:value-of select="LogicalModule" />.Collections.<xsl:text/>
    <xsl:value-of select="LogicalName" />Collection  retVal = new <xsl:text/>
  <xsl:value-of select="LogicalPackage" />.<xsl:value-of select="LogicalModule" />.Collections.<xsl:text/>
    <xsl:value-of select="LogicalName" />Collection();

        return retVal;
      }
      set
      {
        _<xsl:value-of select="LogicalName" /> = value;
      }
    }
  </xsl:template >
  
  
  <xsl:template match="Constraints">
    <xsl:apply-templates  select="Constraint[Type = 'PRIMARY KEY']" mode="primarykey" />
    <xsl:apply-templates  select="Constraint[Type = 'UNIQUE']" mode="uniquekey"/>
  </xsl:template >

  <xsl:template match="Constraint" mode="primarykey">
    
    ///
    ///  Implementation of the tables <xsl:value-of select="Type" /><xsl:value-of select="' '" /><xsl:value-of select="Name" />
    ///
    <xsl:apply-templates  select="Fields" mode="constructor"/>
  </xsl:template >
  <xsl:template match="Constraint" mode="uniquekey">

          ///
          ///  Unique key implementations for getting records<xsl:value-of select="Name" />
          ///
    <xsl:apply-templates  select="Fields" mode="uniquekey"/>
  </xsl:template >  
  
  <xsl:template match="Fields" mode="uniquekey">
          public <xsl:value-of select="../../../LogicalName" /> get<xsl:value-of select="../../../LogicalName" /> (<xsl:apply-templates  select="Field" mode="paramList"/>)
          {
            <xsl:value-of select="../../../LogicalName" /> retVal = new <xsl:value-of select="../../../LogicalName" />();

            return retVal;
          }
  </xsl:template >
  
  <xsl:template match="Fields" mode="constructor">
          public <xsl:value-of select="../../../LogicalName" /><xsl:text>(</xsl:text>
		  <xsl:apply-templates  select="key('fields', FieldReference/@IDREF)" mode="paramList"/>
      <xsl:apply-templates  select="key('allfields', FieldReference/@IDREF)" mode="paramList"/>)
          {
          }
  </xsl:template >
  
    <xsl:template match="Field" mode="paramList">
        <xsl:if test="position() != 1" >, </xsl:if>
        <xsl:apply-templates select="LogicalType/LogicalDataType" /><xsl:value-of select="' '" /><xsl:value-of select="LogicalName" />
  </xsl:template >
  
  
  <xsl:template match="Fields">
    <xsl:apply-templates  select="Field" />
  </xsl:template >

  <xsl:template match="Field">
      
    ///
    /// This field represents the data entity field <xsl:value-of select="DBName" />
          ///
          private <xsl:apply-templates select="LogicalType/LogicalDataType" /> _<xsl:value-of select="LogicalName" />;
          public <xsl:apply-templates select="LogicalType/LogicalDataType" /><xsl:value-of select="' '" /><xsl:value-of select="LogicalName" /><xsl:if test="/Entity/LogicalName = LogicalName"><xsl:value-of select="LogicalType/Name" /></xsl:if>
          {
            get
            {
              return _<xsl:value-of select="LogicalName" />;
            }
            set
            {
              _<xsl:value-of select="LogicalName" /> = value;
            }
          }
  </xsl:template >

  <xsl:template match="LogicalType/LogicalDataType[text() = '_integer' ]" >int</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_text']" >string</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_character']" >string</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_imaginary']" >object</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_real' and //parent::Precision/text() &gt; '0']" >double</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_real' and //parent::Precision/text() = '0']" >float</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_boolean']" >bool</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_temporal']" >DateTime</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_geolocation']" >object</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_guid']" >System.Guid</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_bytesequence']" >byte&#91;&#93;</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_variant']" >object</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_object']" >object</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_xml']" >System.Xml.XmlDocument</xsl:template >



</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2006. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\..\..\..\..\menumaster\data\test\AllModule.xml" htmlbaseurl="" outputurl="" processortype="saxon8" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="" ><advancedProp name="sInitialMode" value=""/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bSchemaAware" value="true"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/><advancedProp name="bTinyTree" value="true"/><advancedProp name="bWarnings" value="true"/><advancedProp name="bUseDTD" value="false"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition><template match="/"></template></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->