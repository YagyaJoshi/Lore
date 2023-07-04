
        jssor_1_slider_init = function() {
            
            var jssor_1_options = {
              $AutoPlay: true,
              $AutoPlaySteps: 4,
              $SlideDuration: 160,
              $SlideWidth:285,
              $SlideSpacing: 3,
              $Cols: 5,
              $ArrowNavigatorOptions: {
                $Class: $JssorArrowNavigator$,
                $Steps: 5
              },
              $BulletNavigatorOptions: {
                $Class: $JssorBulletNavigator$,
                $SpacingX: 5,
                $SpacingY: 5
              }
            };
            
            var jssor_1_slider = new $JssorSlider$("jssor_1", jssor_1_options);
            
            //responsive code begin
            //you can remove responsive code if you don't want the slider scales while window resizing
            function ScaleSlider() {
                var refSize = jssor_1_slider.$Elmt.parentNode.clientWidth;
                if (refSize) {
                    refSize = Math.min(refSize, 1100);
                    jssor_1_slider.$ScaleWidth(refSize);
					
                }
                else {
                    window.setTimeout(ScaleSlider, 30);
					$Cols: 2
                }
				
            }
            ScaleSlider();
            $Jssor$.$AddEvent(window, "load", ScaleSlider);
            $Jssor$.$AddEvent(window, "resize", ScaleSlider);
            $Jssor$.$AddEvent(window, "orientationchange", ScaleSlider);
            //responsive code end
        };
		
		
		
		
		jssor_2_slider_init = function() {
            
            var jssor_2_options = {
               $AutoPlay: true,
              $AutoPlaySteps: 4,
              $SlideDuration: 160,
              $SlideWidth:285,
              $SlideSpacing: 3,
              $Cols: 5,
              $ArrowNavigatorOptions: {
                $Class: $JssorArrowNavigator$,
                $Steps: 5
              },
              $BulletNavigatorOptions: {
                $Class: $JssorBulletNavigator$,
                $SpacingX: 5,
                $SpacingY: 5
              }
            };
            
            var jssor_2_slider = new $JssorSlider$("jssor_2", jssor_2_options);
            
            //responsive code begin
            //you can remove responsive code if you don't want the slider scales while window resizing
            function ScaleSlider1() {
                var refSize = jssor_2_slider.$Elmt.parentNode.clientWidth;
                if (refSize) {
                    refSize = Math.min(refSize, 1100);
                    jssor_2_slider.$ScaleWidth(refSize);
                }
                else {
                    window.setTimeout(ScaleSlider1, 30);
                }
            }
            ScaleSlider1();
            $Jssor$.$AddEvent(window, "load", ScaleSlider1);
            $Jssor$.$AddEvent(window, "resize", ScaleSlider1);
            $Jssor$.$AddEvent(window, "orientationchange", ScaleSlider1);
            //responsive code end
        };
		
		
		
		jssor_3_slider_init = function() {
            
            var jssor_3_options = {
              $AutoPlay: true,
              $AutoPlaySteps: 4,
              $SlideDuration: 160,
              $SlideWidth:285,
              $SlideSpacing: 3,
              $Cols: 5,
              $ArrowNavigatorOptions: {
                $Class: $JssorArrowNavigator$,
                $Steps: 5
              },
              $BulletNavigatorOptions: {
                $Class: $JssorBulletNavigator$,
                $SpacingX: 5,
                $SpacingY: 5
              }
            };
            
            var jssor_3_slider = new $JssorSlider$("jssor_3", jssor_3_options);
            
            //responsive code begin
            //you can remove responsive code if you don't want the slider scales while window resizing
            function ScaleSlider2() {
                var refSize = jssor_3_slider.$Elmt.parentNode.clientWidth;
                if (refSize) {
                    refSize = Math.min(refSize, 1100);
                    jssor_3_slider.$ScaleWidth(refSize);
                }
                else {
                    window.setTimeout(ScaleSlider2, 30);
                }
            }
            ScaleSlider2();
            $Jssor$.$AddEvent(window, "load", ScaleSlider2);
            $Jssor$.$AddEvent(window, "resize", ScaleSlider2);
            $Jssor$.$AddEvent(window, "orientationchange", ScaleSlider2);
            //responsive code end
        };	


		jssor_4_slider_init = function() {
            
            var jssor_4_options = {
              $AutoPlay: true,
              $AutoPlaySteps: 4,
              $SlideDuration: 160,
              $SlideWidth:370,
              $SlideSpacing: 3,
              $Cols: 5,
              $ArrowNavigatorOptions: {
                $Class: $JssorArrowNavigator$,
                $Steps: 5
              },
              $BulletNavigatorOptions: {
                $Class: $JssorBulletNavigator$,
                $SpacingX: 5,
                $SpacingY: 5
              }
            };
            
            var jssor_4_slider = new $JssorSlider$("jssor_4", jssor_4_options);
            
            //responsive code begin
            //you can remove responsive code if you don't want the slider scales while window resizing
            function ScaleSlider3() {
                var refSize = jssor_4_slider.$Elmt.parentNode.clientWidth;
                if (refSize) {
                    refSize = Math.min(refSize, 1100);
                    jssor_4_slider.$ScaleWidth(refSize);
                }
                else {
                    window.setTimeout(ScaleSlider3, 30);
                }
            }
            ScaleSlider3();
            $Jssor$.$AddEvent(window, "load", ScaleSlider3);
            $Jssor$.$AddEvent(window, "resize", ScaleSlider3);
            $Jssor$.$AddEvent(window, "orientationchange", ScaleSlider3);
            //responsive code end
        };	
		
		
		jssor_5_slider_init = function() {
            
            var jssor_5_options = {
              $AutoPlay: true,
              $AutoPlaySteps: 4,
              $SlideDuration: 160,
              $SlideWidth:370,			  
              $SlideSpacing: 3,
              $Cols: 5,
              $ArrowNavigatorOptions: {
                $Class: $JssorArrowNavigator$,
                $Steps: 5
              },
              $BulletNavigatorOptions: {
                $Class: $JssorBulletNavigator$,
                $SpacingX: 5,
                $SpacingY: 5
              }
            };
            
            var jssor_5_slider = new $JssorSlider$("jssor_5", jssor_5_options);
            
            //responsive code begin
            //you can remove responsive code if you don't want the slider scales while window resizing
            function ScaleSlider4() {
                var refSize = jssor_5_slider.$Elmt.parentNode.clientWidth;
                if (refSize) {
                    refSize = Math.min(refSize, 1100);
                    jssor_5_slider.$ScaleWidth(refSize);
                }
                else {
                    window.setTimeout(ScaleSlider4, 30);
                }
            }
            ScaleSlider4();
            $Jssor$.$AddEvent(window, "load", ScaleSlider4);
            $Jssor$.$AddEvent(window, "resize", ScaleSlider4);
            $Jssor$.$AddEvent(window, "orientationchange", ScaleSlider4);
            //responsive code end
        };
		
			
		
    