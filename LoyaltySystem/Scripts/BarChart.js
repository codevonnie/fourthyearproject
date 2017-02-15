function DisplayChart() {

    document.getElementById("HeaderTitle").innerText = "Quietest Months";
    document.getElementById('chart').style.display = 'block';

    var data =[];

    //Loop through the list of Objects and add each one to the array
    for (var key in arr) {
        if (arr.hasOwnProperty(key)) {
            var val = arr[key];
            data.push(val);                               
        }
    }


    var margin = { top: 40, right: 20, bottom: 30, left: 100 },
        width = 960 - margin.left - margin.right,
        height = 500 - margin.top - margin.bottom;

    var formatPercent = d3.format("");

    var x = d3.scale.ordinal()
        .rangeRoundBands([0, width], .1);

    var y = d3.scale.linear()
        .range([height, 0]);

    var xAxis = d3.svg.axis()
        .scale(x)
        .orient("bottom");

    var yAxis = d3.svg.axis()
        .scale(y)
        .orient("left")
        .tickFormat(formatPercent);



    var svg = d3.select("#chart").append("svg").classed("center-block", true)
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");


    // Pass the Data array into the functions
    x.domain(data.map(function (d) { return d.Month; }));
    y.domain([0, d3.max(data, function (d) { return d.Visits; })]);

    svg.append("g")
        .attr("class", "x axis")
        .attr("transform", "translate(0," + height + ")")
        .call(xAxis);

    svg.append("g")
        .attr("class", "y axis")
        .call(yAxis)
      .append("text")
        .attr("transform", "rotate(-90)")
        .attr("y", 10)
        .attr("dy", ".71em")
        .style("text-anchor", "end")
        .text("Visits");

    svg.selectAll(".bar")
        .data(data)
      .enter().append("rect")
        .attr("class", "bar")
        .attr("x", function (d) { return x(d.Month); })
        .attr("width", x.rangeBand())
        .attr("y", function (d) { return y(d.Visits); })
        .attr("height", function (d) { return height - y(d.Visits); })
}

function type(d) {
    d.Visits =+d.Visits;
    return d;
}