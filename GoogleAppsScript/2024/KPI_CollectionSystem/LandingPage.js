function setPageVies(pageViews) {
    sheet.getRange(todayRow, PropertiesService.getScriptProperties().getProperty("COLUMN_PAGE_VIEWS")).setValue(pageViews);
}

function getPageViews() {
    let metric = AnalyticsData.newMetric();

    metric.name = "screenPageViews";

    let dimension = AnalyticsData.newDimension();

    dimension.name = "date";

    let dateRange = AnalyticsData.newDateRange();

    dateRange.startDate = PropertiesService.getScriptProperties().getProperty("DATE_START_PV");

    dateRange.endDate = "today";

    let request = AnalyticsData.newRunReportRequest();

    request.dimensions = [dimension];

    request.metrics = [metric];

    request.dateRanges = dateRange;

    const PROPERTY_ID = PropertiesService.getScriptProperties().getProperty("ANALYTICS_ID_PRPERTY");

    const RESPONSE = JSON.parse(AnalyticsData.Properties.runReport(request, "properties/" + PROPERTY_ID).toString());

    let pageViews = 0;

    for (let i = 0; i < RESPONSE.rows.length; i++) {
        pageViews += parseInt(RESPONSE.rows[i].metricValues[0].value);
    }

    return pageViews;
}