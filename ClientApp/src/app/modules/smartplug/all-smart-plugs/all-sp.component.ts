import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ApexOptions } from 'apexcharts';
import { ApexAxisChartSeries, ChartComponent } from 'ng-apexcharts';
import { environment } from 'src/environments/environment';
import { HomeService } from '../../home/_services/home.service';

export type DailyChartOpt = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  options: ApexOptions;
  legend: ApexLegend;
  plotOptions: ApexPlotOptions;
  responsive: ApexResponsive;
};

export type WeeklyChartOpt = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  options: ApexOptions;
  legend: ApexLegend;
};

@Component({
  selector: 'sh-all-sp',
  templateUrl: './all-sp.component.html',
  styleUrls: ['./all-sp.component.scss'],
})
export class AllSmartPlugs implements OnInit {
  @ViewChild('dailyChart') dailyChart: ChartComponent;
  @ViewChild('weeklyChart') weeklyChart: ChartComponent;

  public dailyChartOptions: Partial<DailyChartOpt>;
  public weeklyChartOptions: Partial<WeeklyChartOpt>;

  smartPlugs: any[] = [];
  labels: string[] = [];
  data: number[] = [];
  total: number;
  time: string = this.translate.instant('SMART_PLUG.TIME.LAST_24_HOURS');

  constructor(
    public translate: TranslateService,
    private router: Router,
    private home: HomeService,
    private http: HttpClient
  ) {}

  selectTime(type) {

    this.time = type;

    let reqTime = '';
    switch (type) {
      case this.translate.instant('SMART_PLUG.TIME.LAST_24_HOURS'):
        reqTime = '24h';
        break;
      case this.translate.instant('SMART_PLUG.TIME.ONE_WEEK'):
        reqTime = '1w';
        break;
      case this.translate.instant('SMART_PLUG.TIME.ONE_MONTH'):
        reqTime = '1m';
        break;
      case this.translate.instant('SMART_PLUG.TIME.TWO_MONTHS'):
        reqTime = '2m';
        break;
      default:
        reqTime = '24h';
        break;
    }

    this.http
      .post(`${environment.apiUrl}/Smartplug`, {
        format: reqTime,
        fromDate: '',
        toDate: '',
      })
      .subscribe(
        (res: any) => {

        this.smartPlugs = res.outputs;

        this.total = res.total;

        this.labels = [];
        this.data = [];

        this.smartPlugs.forEach((sp) => {
          this.data.push(sp.totalWatt);
          this.labels.push(sp.baseDevice.name);
        });

        this.setChart();
      },
      err => {}
      );
  }

  ngOnInit() {
    this.http
      .post(`${environment.apiUrl}/Smartplug`, {
        format: '24h',
        fromDate: '',
        toDate: '',
      })
      .subscribe((res: any) => {

        this.smartPlugs = res.outputs;
        this.total = res.total;

        this.labels = [];
        this.data = [];

        this.smartPlugs.forEach((sp) => {
          this.data.push(sp.totalWatt);
          this.labels.push(sp.baseDevice.name);
        });

        this.setChart();
      });
  }

  goToDetails(id) {
    this.router.navigateByUrl(`/smartplug/details/${id}`);
  }

  setChart() {
    this.dailyChartOptions = {
      plotOptions: {
        pie: {
          donut: {
            labels: {
              show: true,
              name: {
                color: 'var(--text-color)',
                fontFamily: 'Vazir',
              },
              value: {
                color: 'var(--text-color)',
                fontSize: '1.6rem',
              },
              total: {
                show: true,
                showAlways: true,
                label: this.translate.instant('SMART_PLUG.TOTAL') + ' (Kwh)',
                color: 'var(--text-color)',
              },
            },
          },
        },
      },
      options: {
        series: this.data,
        labels: this.labels,
        dataLabels: {
          enabled: false,
        },
      },
      chart: {
        type: 'donut',
        height: '300px',
        parentHeightOffset: 0,
      },
      legend: {
        show: true,
        position: 'left',
        fontFamily: 'Vazir',
        fontSize: '20px',
        labels: {
          colors: 'var(--text-color)',
        },
      },
      responsive: {
        breakpoint: 992,
        options: {
          legend: {
            show: true,
            position: 'bottom',
            fontFamily: 'Vazir',
            fontSize: '20px',
            labels: {
              colors: 'var(--text-color)',
            },
          },
        },
      },
    };
  }
}
