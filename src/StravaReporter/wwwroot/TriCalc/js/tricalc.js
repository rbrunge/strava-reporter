$(document).ready(function () {

    var transitionArray = [];
    for (i = 0; i < 60 * 9.5; i++) {
        var d1 = (new Date).clearTime()
                .addSeconds(30 + i);
        transitionArray.push(d1.toString('mm:ss'));
    }

    var swimPaceArray = [];
    for (i = 0; i <= 60 * 4; i++) {
        var d1 = (new Date).clearTime()
                .addSeconds(45 + i);
        swimPaceArray.push(d1.toString('mm:ss'));
    }

    var rideSpeedArray = [];
    for (i = 18.0; i <= 60.0; i += .01) {
        rideSpeedArray.push(parseFloat(Math.round(i * 100) / 100).toFixed(2));
    }

    var runPaceArray = [];
    for (i = 0; i < 60 * 9.5; i++) {
        var d1 = (new Date).clearTime()
                .addSeconds(60 + i);
        runPaceArray.push(d1.toString('mm:ss'));
    }

    $('#swim-pace-slider').slider({
        formater: function (value) {
            return swimPaceArray[value];
        },
        min: 0,
        max: swimPaceArray.length - 1
    });

    $('#transition-one-index').slider({
        formater: function (value) {
            return transitionArray[value];
        },
        min: 0,
        max: transitionArray.length - 1

    });

    $('#ride-speed-slider').slider({
        formater: function (value) {
            return rideSpeedArray[value];
        },
        min: 0,
        max: rideSpeedArray.length - 1
    });

    $('#transition-two-index').slider({
        formater: function (value) {
            return transitionArray[value];
        },
        min: 0,
        max: transitionArray.length - 1
    });

    $('#run-pace-slider').slider({
        formater: function (value) {
            return runPaceArray[value];
        },
        min: 0,
        max: runPaceArray.length - 1
    });

    function toHms(seconds) {
        var d = (new Date).clearTime()
            .addSeconds(seconds);

        if (seconds >= 3600)
            return d.toString('H:mm:ss');

        return d.toString('mm:ss');
    }

    // var ViewModel = function (swimPace, transitionOne, rideSpeed, transitionTwo, runPace, previousResults) {
    var ViewModel = function (previousResults) {
        var that = this;
        this.swimPace = ko.observable();
        this.transitionOneIndex = ko.observable();
        this.rideSpeed = ko.observable();
        this.transitionTwoIndex = ko.observable();
        this.runPace = ko.observable();
        this.currentPreset = ko.observable('standard');
        this.currentPresetName = ko.observable('');

        this.load = function (swimPace, transitionOne, rideSpeed, transitionTwo, runPace) {
            this.swimPace(swimPaceArray.indexOf(swimPace));
            this.transitionOneIndex(transitionArray.indexOf(transitionOne));
            this.rideSpeed(rideSpeedArray.indexOf(parseFloat(Math.round(rideSpeed * 100) / 100).toFixed(2)));
            this.transitionTwoIndex(transitionArray.indexOf(transitionTwo));
            this.runPace(runPaceArray.indexOf(runPace));
        };

        this.presets = ko.observableArray(previousResults);

        this.distances = ko.observableArray([
            { "name": "4-18-4", "description": "", "distances": { "swim": "400", "ride": "18000", "run": "4000" } },
            { "name": "Sprint", "description": "", "distances": { "swim": "750", "ride": "20000", "run": "5000" } },
            { "name": "Olympic", "description": "", "distances": { "swim": "1500", "ride": "40000", "run": "10000" } },
            { "name": "70.3 (half)", "description": "", "distances": { "swim": "1900", "ride": "90000", "run": "21100" } },
            { "name": "Full", "description": "", "distances": { "swim": "3800", "ride": "180000", "run": "42195" } }
        ]);


        that.usePreset = function (presetName, top) {
            if (top == null || typeof (top) === 'undefined') {
                top = 0;
            }
            for (var item in this.presets()) {
                var p;
                if (typeof (presetName) === 'undefined') {
                    p = this.presets()[0];
                    presetName = p.name;
                } else {
                    p = this.presets()[item];
                }
                if (p.name === presetName) {
                    this.currentPreset(presetName);
                    this.currentPresetName(p.title);
                    this.load(p.data.swim, p.data.t1, p.data.ride, p.data.t2, p.data.run);
                    /*
                                        $('html, body').animate({
                                            scrollTop: top 
                                    }, 800);
                    */
                    return;
                }
            }
        }

        this.onClickPreset = function (presetName, element) {
            console.log(element.name);
            //$(element).hide();
            // $("#calculator").detach().appendTo(element);
            var t = $(element)[0].currentTarget;
            //$('<div style="background-color: red;">asdda</div>').appendTo(t);
            $("#calculator").detach().insertAfter(t);
            var n = presetName.name;
            return that.usePreset(n, $(element)[0].pageX);
        };


        var hmsToSecondsOnly = function (str) {
            var p = str.split(':'),
                s = 0, m = 1;

            while (p.length > 0) {
                s += m * parseInt(p.pop(), 10);
                m *= 60;
            }

            return s;
        }

        this.getTransitionTimeOne = function () {
            return hmsToSecondsOnly(transitionArray[this.transitionOneIndex()]);
        }

        this.getTransitionTimeTwo = function () {
            return hmsToSecondsOnly(transitionArray[this.transitionTwoIndex()]);
        }

        this.getSwimInSeconds = function (meters) {
            return meters * hmsToSecondsOnly(swimPaceArray[this.swimPace()]) / 100;
        }

        // =L9/$D$3/(1440/60)
        this.getRideInSeconds = function (meters) {
            return ((meters / 1000) / (rideSpeedArray[this.rideSpeed()])) * 60 * 60;
        }

        this.getRunInSeconds = function (meters) {
            return hmsToSecondsOnly(runPaceArray[this.runPace()]) * (meters / 1000)
        }

        // ride: L9/$D$3/(1440/60)
        this.calculateTotal = function (swimDistance, rideDistance, runDistance) {
            var seconds = this.getSwimInSeconds(swimDistance)
                        + this.getTransitionTimeOne()
                        + this.getRideInSeconds(rideDistance)
                        + this.getTransitionTimeTwo()
                        + this.getRunInSeconds(runDistance)
            return seconds;
        }

        this.getPercentage = function (swimDistance, rideDistance, runDistance, x) {
            var total = this.calculateTotal(swimDistance, rideDistance, runDistance);
            var discipline = 0;
            switch (x) {
                case "swim":
                    discipline = this.getSwimInSeconds(swimDistance);
                    break;
                case "ride":
                    discipline = this.getRideInSeconds(rideDistance);
                    break;
                case "run":
                    discipline = this.getRunInSeconds(runDistance);
                    break;
                case "t1":
                    discipline = this.getTransitionTimeOne();
                    break;
                case "t2":
                    discipline = this.getTransitionTimeTwo();
                    break;
            }
            return Math.round(100 * (discipline * 10 / total)) / 10 + ' %';
        }

        this.swimPaceFormatted = ko.computed(function () {
            return swimPaceArray[this.swimPace()] + " min/100 m";
        }, this);

        this.rideSpeedFormatted = ko.computed(function () {
            return rideSpeedArray[this.rideSpeed()] + " km/h";
        }, this);

        this.runPaceFormatted = ko.computed(function () {
            return runPaceArray[this.runPace()] + " min/km";
        }, this);

        this.toggleHelp = function () {
            // $("#help").toggle();
            $("#help").toggle("slow");
        }

        this.swimChange = function (element, amount) {
            if (typeof (amount) === 'undefined') {
                amount = 1;
            }

            if (element() + amount > element().length) {
                element(element().length - 1);
                return;
            }

            if (element() - amount < 0) {
                element(0);
                return;
            }

            element(element() + amount);
        }
    };

    ko.bindingHandlers.sliderValue = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var value = valueAccessor();
            $(element).slider('setValue', value())
            $(element).on('slide', function (ev) {
                value(ev.value);
            });
        },
        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var value = valueAccessor();
            $(element).slider('setValue', value());
        }
    };

    // Display value as: hh:mm:ss
    ko.bindingHandlers.hmsValue = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var value = valueAccessor(),
                allBindings = allBindingsAccessor();
            var valueUnwrapped = ko.utils.unwrapObservable(value);
            var newValue = toHms(valueUnwrapped.toString());
            if (element.nodeName === "INPUT") {
                $(element).val(newValue);
            } else {
                $(element).text(newValue);
            }
        }
    };

    ko.bindingHandlers.dateValue = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var value = valueAccessor(),
                allBindings = allBindingsAccessor();
            var valueUnwrapped = ko.utils.unwrapObservable(value);
            var newValue = valueUnwrapped.toString();

            var d = newValue.substring(0, 4) + '-' + newValue.substring(4, 6) + '-' + newValue.substring(6, 8);
            newValue = moment(new Date(d)).format('ll');

            if (element.nodeName === "INPUT") {
                $(element).val(newValue);
            } else {
                $(element).text(newValue);
            }
        }
    };


    var vm = new ViewModel(previousResults);
    vm.usePreset();
    ko.applyBindings(vm);
});
