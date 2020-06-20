(function() {

	//点集，描边样式，填充样式，是否描边，是否填充，返回一个canvas.
	var Polygon = function(points, strokeStyle, fillStyle, isStroke, isFill) {
		this.points = points;
		this.strokeStyle = strokeStyle;
		this.fillStyle = fillStyle;
		this.isFill = isFill;

		this.draw = function(id) {
			var canvas = document.createElement('canvas');
			var context = canvas.getContext('2d');
			var maxX = 0,
				maxY = 0;
				console.log(this.points.length)
			for (var i = 0; i < this.points.length; i++) {
				if (this.points[i][0] > maxX)
					maxX = this.points[i][0];
				if (this.points[i][1] > maxY)
					maxY = this.points[i][1];
			}
			canvas.height = maxY;
			canvas.width = maxX;
			context.beginPath();
			context.strokeStyle = this.strokeStyle;
			context.fillStyle = this.fillStyle;
			context.moveTo(this.points[0][0], this.points[0][1]);
			for (var i = 1; i < this.points.length; i++)
				context.lineTo(this.points[i][0], this.points[i][1]);
			context.closePath();
			if (isFill)
				context.fill();
			if (isStroke)
				context.stroke();
			canvas.style.position = "absolute";
			canvas.id = id;
			document.getElementById('stage').appendChild(canvas);
			return canvas;
		}
	}
	//滑动条的高，宽，段数，每段的描述文字，游标尺寸
	function Slide(height, width, blockCount, descriptions, cursorUnit) {
		this.height = height;
		this.width = width;
		this.cursorUnit = cursorUnit;

		//定义游标形状
		this.cursor = [
			[cursorUnit, 0],
			[2 * cursorUnit, cursorUnit],
			[2 * cursorUnit, 3 * cursorUnit],
			[0, 3 * cursorUnit],
			[0, cursorUnit],
			[cursorUnit, 0]
		];

		//定义颜色
		this.colors = ['#F56C6C', '#E6A23C', '#67C23A'];

		//横轴
		var tmp;
		tmp = new Polygon([
			[0, 0],
			[width, 0],
			[width, 10]
		], 'blue', undefined, true, false);
		this.axis = tmp.draw('axis');

		//游标的数组
		this.points = [];
		console.log("with "+ blockCount)
		for (var i = 0, d = width / blockCount; i < blockCount; i++) {

			tmp = new Polygon(this.cursor, undefined, this.colors[i], false, true);
			this.points[i] = tmp.draw(i);
			this.points[i].style.top = this.height + 'px';
			this.points[i].style.left = d * (i + 1) - this.cursorUnit + 'px';

			//每个游标的描述块
			var description = document.createElement('div');
			//每个游标的占比
			var ratio = document.createElement('p');
			//每个游标的描述内容
			var content = document.createElement('p');

			description.id = i + 'description';
			ratio.id = i + 'ratio';
			content.id = i + 'content';
			description.style.position = "absolute";
			description.style.width = this.width;
			description.style.left = this.points[i].style.left;
			description.style.top = this.height + this.cursorUnit + 'px';
			description.style.color = this.colors[i];
			description.style.fontFamily = '微软雅黑';
			description.style.visibility = 'hidden';
			document.getElementById('stage').appendChild(description);
			description.appendChild(ratio);
			description.appendChild(content);

		}

		//横轴填色
		this.axisContext = this.axis.getContext('2d');
		for (var i = 0, d = width / blockCount; i < blockCount; i++) {
			this.axisContext.fillStyle = this.colors[i];
			this.axisContext.fillRect(d * i, 0, d, this.height);
		}

		//添加游标响应事件
		this.addListener = function() {
			//当前拖动游标
			var drager = undefined;
			//鼠标前一个时刻的横坐标
			var pre = undefined;
			//当前拖动的游标可活动范围的最左坐标
			var left = undefined;
			//当前拖动的游标可活动范围的最右坐标
			var right = undefined;
			//当前拖动游标的ID
			var id = undefined;
			var temp = this;

			//每个游标添加鼠标按下事件
			for (var i = 0; i < temp.points.length; i++) {
				temp.points[i].onmousedown = function(e) {
					if (i != temp.points.length - 1)
						drager = this;
					pre = e.clientX;
					id = parseInt(this.id);
					left = (id == 0 ? temp.cursorUnit : parseInt(temp.points[id - 1].style.left) + 2 * temp.cursorUnit);
					right = (id == temp.points.length - 1 ? parseInt(temp.width) - 3 * temp.cursorUnit : parseInt(temp.points[id +
						1].style.left) - 2 * temp.cursorUnit);
					document.getElementById(id + 'description').style.visibility = 'visible';
				}
			}

			document.onmouseup = function(e) {
				if (drager != undefined) {
					document.getElementById(id + 'description').style.visibility = 'hidden';
					drager = undefined;
					pre = undefined;
					id = undefined;
				}
			}

			document.onmousemove = function(e) {
				if (drager != undefined) {
					var tmp = parseInt(drager.style.left) + e.clientX - pre;
					if (tmp >= left && tmp <= right) {
						drager.style.left = tmp + 'px';
						var description = document.getElementById(id + 'description');
						description.style.left = tmp + 'px';
						document.getElementById(id + 'ratio').innerHTML = Math.round((tmp - left + 2 * temp.cursorUnit) / temp.width *
							10000) / 100 + '%';
						document.getElementById(id + 1 + 'ratio').innerHTML = Math.round((right - tmp + 2 * temp.cursorUnit) / temp.width *
							10000) / 100 + '%';
						temp.axisContext.fillStyle = temp.colors[id];
						temp.axisContext.fillRect(left, 0, tmp - left + temp.cursorUnit, temp.height);
						temp.axisContext.fillStyle = temp.colors[id + 1];
						temp.axisContext.fillRect(tmp + temp.cursorUnit, 0, right - tmp, temp.height);
					}
					pre = e.clientX;
				}
			}
		}
	}

	new Slide(15, 300, 3, ['不做处理并阀值', '手动合并阀值', '自动合并阀值'], 3).addListener();
})();
