<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" indent="yes" />

  <xsl:template match="/" >

    
    <xsl:apply-templates select="Entity"/>
    

  </xsl:template>


  <xsl:template match="Entity">

    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.ComponentModel;
    using System;

    namespace <xsl:value-of select="Schema" />.<xsl:value-of select="LogicalPackage" />.<xsl:value-of select="LogicalModule" />
    {
    
      [Table(Name = "<xsl:value-of select="Schema" />.<xsl:value-of select="DBName" />")]
      public class <xsl:value-of select="LogicalName" /> : INotifyPropertyChanging, INotifyPropertyChanged
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
    
    <xsl:apply-templates select="Relation" />
  
    #endregion
  </xsl:template >
  
  
  <xsl:template match="Relation">
    private <xsl:value-of select="Entity/Schema" />.<xsl:value-of select="Entity/LogicalPackage" />.<xsl:value-of select="Entity/LogicalModule" />.Collections.<xsl:value-of select="Entity/LogicalName" />Collection _<xsl:value-of select="LogicalName" />;
    public <xsl:value-of select="Entity/Schema" />.<xsl:value-of select="Entity/LogicalPackage" />.<xsl:value-of select="Entity/LogicalModule" />.Collections.<xsl:value-of select="Entity/LogicalName" />Collection <xsl:value-of select="LogicalName" />
    {
      get
      {
        <xsl:value-of select="Entity/Schema" />.<xsl:value-of select="Entity/LogicalPackage" />.<xsl:value-of select="Entity/LogicalModule" />.Collections.<xsl:value-of select="Entity/LogicalName" />Collection  retVal = new <xsl:value-of select="Entity/Schema" />.<xsl:value-of select="Entity/LogicalPackage" />.<xsl:value-of select="Entity/LogicalModule" />.Collections.<xsl:value-of select="Entity/LogicalName" />Collection();

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
          public <xsl:value-of select="../../../LogicalName" /> (<xsl:apply-templates  select="Field" mode="paramList"/>)
          {
          }
  </xsl:template >
  
    <xsl:template match="Field" mode="paramList">
        <xsl:if test="position() != 1" >, </xsl:if>
        <xsl:value-of select="LogicalType/Name" /><xsl:value-of select="' '" /><xsl:value-of select="LogicalName" />
  </xsl:template >
  
  
  <xsl:template match="Fields">
    <xsl:apply-templates  select="Field" />
  </xsl:template >

  <xsl:template match="Field">
      
          /// 
          /// This field represents the data entity field <xsl:value-of select="DBName" />
    ///
    private <xsl:apply-templates  select="." mode="typedecalration" /> _<xsl:value-of select="LogicalName" />;
    public <xsl:apply-templates  select="." mode="typedecalration" /><xsl:value-of select="' '" /><xsl:value-of select="LogicalName" /><xsl:if test="/Entity/LogicalName = LogicalName">
      <xsl:value-of select="LogicalType/Name" />
    </xsl:if>
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
  <xsl:template match="Field" mode="typedecalration">
    <xsl:choose>
      <xsl:when test="(Nullable = 'true') and (LogicalType/Primative = 'true')" >
        System.Nullable&lt;<xsl:value-of select="LogicalType/Name" />&gt;
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="LogicalType/Name" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  
  </xsl:stylesheet>