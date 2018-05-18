var colors = d3.schemeCategory10 /*[
    'rgba(255, 0, 0, 0.5)',
    'rgba(0, 255, 0, 0.5)',
    'rgba(0, 0, 255, 0.5)',
    'rgba(126, 126, 126, 0.5)',
    'rgba(0, 126, 126, 0.5)',
    'rgba(126, 126, 0, 0.5)',
    'rgba(126, 0, 126, 0.5)',
    'rgba(55, 126, 55, 0.5)'
]*/;
function getLabels(data, modalNames, prefix){
    var labels = [];
    data.map(function(b) {
        if (modalNames.has(b["ModalName"])){
            if(prefix){
                if(!labels.has(b["ModalName"]+" - "+b["SubModalName"]))
                    labels.push(b["ModalName"]+" - "+b["SubModalName"]);
            }
            else{
                if(!labels.has(b["SubModalName"]))
                    labels.push(b["SubModalName"]);
            }
        }
    });
    return labels;
}
function getPersonData(data, modalNames, prefix, durationScene){
    var persons = {};
    data.map(function(b) {
        if (modalNames.has(b["ModalName"])){
            var d = Date.parse("1970-01-01T" + b["TotalDuration"] + "Z");
            var c = new Date(d);
            var s = c;
            //var s = c.getUTCHours()*60*60+c.getUTCMinutes()*60+c.getUTCSeconds()+c.getUTCMilliseconds()/1000.0;
            if(! (b["PersonName"] in persons))
                persons[b["PersonName"]] = {}
            if(durationScene != 0)
                s = s/durationScene;
            if(prefix)
                persons[b["PersonName"]][b["ModalName"]+" - "+b["SubModalName"]] = s;
            else
                persons[b["PersonName"]][b["SubModalName"]] = s;
        }
    });
    return persons;
}
function createBarChartData(persons, labels){
    var barChartData={};
    barChartData.labels = labels;
    barChartData.datasets = [];
    Object.keys(persons).map(function(p, i){
        d = {};
        d.label = p;
        d.borderWidth = 1;
        d.backgroundColor = colors[i]
        d.data = [];
        labels.map(function(l){
            if(l in persons[p]){
                d.data.push(persons[p][l]);
            }else{
                d.data.push(0);
            }
        });
        barChartData.datasets.push(d);
    });
    return barChartData;
}
function createModalIntervalsGraph(intervals, durationScene, name, i){
    var intervalsData={};
    intervalsData.type = 'line';
    intervalsData.data = {};
    intervalsData.data.datasets = [];
    intervalsData.options = {
        animation: {
            duration: 0
        },
        legend : {
            display : false
        },
        scales: {
            xAxes: [{
                type: 'linear',
                position: 'bottom',
                ticks : {
                    beginAtzero :true,
                    min: 0,
                    max: durationScene,
                    callback: function(value, index, values){
                        return parseInt(String(value/60))+":"+parseInt(String((value/60-parseInt(String(value/60)))*60))
                    }
                    
                }
            }],
            yAxes : [{
                display: false,
                ticks : {
                    beginAtZero :true,
                    max : 1
                }
            }]
        }
    };
    Object.keys(intervals).map(function(submodal, iSubmodal){
        
        if(intervals[submodal] != null){
            for(var iInterval in intervals[submodal]){
                d = {};
                d.backgroundColor = colors[iSubmodal%colors.length];
                d.fill = true;
                d.borderWidth = 0;
                d.pointRadius = 0;
                d.labels = [submodal];
                var dStart = Date.parse("1970-01-01T" + intervals[submodal][iInterval]["StartTime"] + "Z");
                var cStart = new Date(dStart);
                var sStart = cStart.getUTCHours()*60*60+cStart.getUTCMinutes()*60+cStart.getUTCSeconds()+cStart.getUTCMilliseconds()/1000.0;
                var dEnd = Date.parse("1970-01-01T" + intervals[submodal][iInterval]["EndTime"] + "Z");
                var cEnd = new Date(dEnd);
                var sEnd = cEnd.getUTCHours()*60*60+cEnd.getUTCMinutes()*60+cEnd.getUTCSeconds()+cEnd.getUTCMilliseconds()/1000.0;

                d.data = [{
                    x: sStart,
                    y: 1
                },{
                    x: sEnd,
                    y: 1
                }];
                intervalsData.data.datasets.push(d);
            }
        }
        
    })
    
    return intervalsData;
}
function createSubModalIntervalsGraph(intervals, durationScene, name, i){
    var intervalsData={};
    intervalsData.type = 'line';
    intervalsData.data = {};
    intervalsData.data.datasets = [];
    intervalsData.options = {
        animation: {
            duration: 0
        },
        legend : {
            display : false
        },
        scales: {
            xAxes: [{
                type: 'linear',
                position: 'bottom',
                ticks : {
                    beginAtzero :true,
                    min: 0,
                    max: durationScene,
                    callback: function(value, index, values){
                        return parseInt(String(value/60))+":"+parseInt(String((value/60-parseInt(String(value/60)))*60))
                    }
                    
                }
                /*time: {
                    min: 0,
                    max: durationScene,
                    unit: 'second',
                    displayFormats: {
                        second: 'mm:ss'
                    }
                }*/
            }],
            yAxes : [{
                display: false
                /*scaleLabel : {
                    display : false
                }*/,
                ticks : {
                    beginAtZero :true,
                    max : 1
                }
            }]
        }
    };
    //console.log(name);
    //console.log();
    for(var iInterval in intervals){
        d = {};
        d.backgroundColor = colors[i%colors.length];
        d.fill = true;
        d.borderWidth = 0;
        d.pointRadius = 0;
        var dStart = Date.parse("1970-01-01T" + intervals[iInterval]["StartTime"] + "Z");
        var cStart = new Date(dStart);
        var sStart = cStart.getUTCHours()*60*60+cStart.getUTCMinutes()*60+cStart.getUTCSeconds()+cStart.getUTCMilliseconds()/1000.0;
        var dEnd = Date.parse("1970-01-01T" + intervals[iInterval]["EndTime"] + "Z");
        var cEnd = new Date(dEnd);
        var sEnd = cEnd.getUTCHours()*60*60+cEnd.getUTCMinutes()*60+cEnd.getUTCSeconds()+cEnd.getUTCMilliseconds()/1000.0;

        d.data = [{
            x: sStart,
            y: 1
        },{
            x: sEnd,
            y: 1
        }];
        intervalsData.data.datasets.push(d);
    }
    
    return intervalsData;
}



function createTreeMap(id, data){
    var svg = d3.select("#"+id),
        width = +svg.attr("width"),
        height = +svg.attr("height");

    var fader = function(color) { return d3.interpolateRgb(color, "#fff")(0.2); },
        /*color = d3.scaleOrdinal(d3.schemeCategory10.map(fader)),*/
        color = d3.scaleOrdinal(colors.map(fader)),
        format = d3.format(",d");

    var treemap = d3.treemap()
        .tile(d3.treemapResquarify)
        .size([width, height])
        .round(true)
        .paddingInner(1);

    //$.getJSON("flare.json")
    //        .done(function(data){
    //d3.json("flare.json", function(error, data) {
      //if (error) throw error;

      var root = d3.hierarchy(data)
          .eachBefore(function(d) { d.data.id = (d.parent ? d.parent.data.id + "." : "") + d.data.name; })
          .sum(sumBySize)
          .sort(function(a, b) { return b.height - a.height || b.value - a.value; });

      treemap(root);

      var cell = svg.selectAll("g")
        .data(root.leaves())
        .enter().append("g")
          .attr("transform", function(d) { return "translate(" + d.x0 + "," + d.y0 + ")"; });

      cell.append("rect")
          .attr("id", function(d) { return d.data.id; })
          .attr("width", function(d) { return d.x1 - d.x0; })
          .attr("height", function(d) { return d.y1 - d.y0; })
          .attr("fill", function(d) { return color(d.parent.data.id); });

      cell.append("clipPath")
          .attr("id", function(d) { return "clip-" + d.data.id; })
        .append("use")
          .attr("xlink:href", function(d) { return "#" + d.data.id; });

      cell.append("text")
          .attr("clip-path", function(d) { return "url(#clip-" + d.data.id + ")"; })
        .selectAll("tspan")
          .data(function(d) { return d.data.name.split(/(?=[A-Z][^A-Z])/g); })
        .enter().append("tspan")
          .attr("x", 4)
          .attr("y", function(d, i) { return 13 + i * 10; })
          .text(function(d) { return d; });

      cell.append("title")
          .text(function(d) { return d.data.id + "\n" + format(d.value); });

      d3.selectAll("input")
          .data([sumBySize, sumByCount], function(d) { return d ? d.name : this.value; })
          .on("change", changed);

      var timeout = d3.timeout(function() {
        d3.select("input[value=\"sumByCount\"]")
            .property("checked", true)
            .dispatch("change");
      }, 2000);

      function changed(sum) {
        timeout.stop();

        treemap(root.sum(sum));

        cell.transition()
            .duration(750)
            .attr("transform", function(d) { return "translate(" + d.x0 + "," + d.y0 + ")"; })
          .select("rect")
            .attr("width", function(d) { return d.x1 - d.x0; })
            .attr("height", function(d) { return d.y1 - d.y0; });
      }

    function sumByCount(d) {
      return d.children ? 0 : 1;
    }

    function sumBySize(d) {
      return d.size;
    }
}