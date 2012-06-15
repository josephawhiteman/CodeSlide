<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" indent="yes" />

  <xsl:template match="/" >
  &lt;?xml version="1.0" encoding="UTF-8" ?&gt;
  &lt;!DOCTYPE jdo PUBLIC     [1]
    "-//Sun Microsystems, Inc.//DTD Java Data Objects Metadata 1.0//EN"
    "http://java.sun.com/dtd/jdo_1_0.dtd"&gt;
  <jdo>
    <package name="com.mediamania.prototype" >
      <xsl:apply-templates select="Entity"/>
    </package>
  </jdo>  
  
  </xsl:template>


  <xsl:template match="Entity">

    &lt;class name="<xsl:value-of select="LogicalName" />" &gt;

    <xsl:apply-templates select="Fields" />
    <xsl:apply-templates select="ChildRelations" />
    
    &lt;/class&gt;

  </xsl:template>

  <xsl:template match="Fields">
    <xsl:apply-templates  select="Field" />
  </xsl:template >


  <xsl:template match="Field">

    &lt;field name="cast" &gt;
      &lt;collection element-type=&quot;<xsl:value-of select="LogicalName" />&quot;/&gt;
    &lt;/field&gt;
  </xsl:template >

  <xsl:template match="ChildRelations">

    #region  Related table accessors

    <xsl:apply-templates select="RelationReference" />

    #endregion
  </xsl:template >
  
  <xsl:template match="RelationReference">

    <xsl:apply-templates select="Relation" />

  </xsl:template >
  
  <xsl:template match="Relation">

    &lt;field name=&quot;<xsl:value-of select="ChildEntity/Entity/LogicalName" />Collection&quot; &gt;
    &lt;collection element-type=&quot;<xsl:value-of select="ChildEntity/Entity/LogicalName" />&quot;/&gt;
    &lt;/field&gt;
  </xsl:template >

  </xsl:stylesheet>