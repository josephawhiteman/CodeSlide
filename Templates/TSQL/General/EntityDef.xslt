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

    --<xsl:value-of select="LogicalPackage" />
    --<xsl:value-of select="LogicalModule" />
    {
      CREATE TABLE <xsl:value-of select="LogicalName" />
    (

    <xsl:apply-templates select="Fields" />
  )
  </xsl:template>

  <xsl:template match="Fields">
    <xsl:apply-templates  select="Field" />
  </xsl:template >

  <xsl:template match="Field">

    <xsl:if test="position() != 1 " >, </xsl:if>
    <xsl:value-of select="LogicalName" /> <xsl:apply-templates select="LogicalType/LogicalDataType" />
  </xsl:template >


  <xsl:template match="LogicalType/LogicalDataType[text() = '_integer' ]" >int</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_text']" >
    <xsl:if test="LogicalType/Encoding != ''" >n</xsl:if>varchar (<xsl:value-of select="Max" /><xsl:text>)</xsl:text>
</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_character']" >
    <xsl:if test="LogicalType/Encoding != ''" >n</xsl:if>char (<xsl:value-of select="Max" />)
  </xsl:template >

  <xsl:template match="LogicalType/LogicalDataType[text() = '_imaginary']" >object</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_real' and //parent::Precision/text() &gt; '0']" >double</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_real' and //parent::Precision/text() = '0']" >float</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_boolean']" >bit</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_temporal']" >datetime</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_geolocation']" >object</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_guid']" >uniqueidentifier</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_bytesequence']" >(<xsl:value-of select="Max" />
    <xsl:text>)</xsl:text>
  </xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_variant']" >sql_variant</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_object']" >object</xsl:template >
  <xsl:template match="LogicalType/LogicalDataType[text() = '_xml']" >xml</xsl:template >


</xsl:stylesheet>