import { element } from 'protractor';
import { NgbDate, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import * as ApexCharts from 'apexcharts';

import {
  ChartComponent,
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexFill,
  ApexAnnotations,
  ApexDataLabels,
  ApexGrid,
  ApexYAxis,
  ApexStroke,
} from 'ng-apexcharts';
import { environment } from 'src/environments/environment';

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis;
  fill: ApexFill;
  annotations: ApexAnnotations;
  dataLabels: ApexDataLabels;
  grid: ApexGrid;
  stroke: ApexStroke;
};

@Component({
  selector: 'sh-smartplug-charts',
  templateUrl: './smartplug-chart.component.html',
  styleUrls: ['./smartplug-chart.component.scss'],
})
export class SmartPlugChartsComponent implements OnInit {
  @Input() name: string = '';
  voltages: any[] = [];
  energies: any[] = [];
  chartData: any[] = [];
  labels: string[] = [];
  labelColors: string[] = [];
  smartPlug: any = {
    totalWatt: 2000,
  };
  hoveredDate: NgbDate | null = null;
  from: Date;
  to: Date;
  tod = new Date()
  shamsiFrom: string;
  shamsiTo: string;

  // fromDate: NgbDate;
  // toDate: NgbDate | null = null;

  chartType: string = this.translate.instant('SMART_PLUG.LOOKUP.ENERGY');
  selectedTime: string = this.translate.instant(
    'SMART_PLUG.TIME.LAST_24_HOURS'
  );
  range: any[] = [];

  x1Peak : string;
  x2Peak : string;

  x1Peak2 : string;
  x2Peak2: string;

  chartMin: number;
  chartMax: number;

  @ViewChild('chart') chart: ChartComponent;
  public chartOptions: Partial<ChartOptions>;

  constructor(
    public translate: TranslateService,
    private http: HttpClient,
    private ar: ActivatedRoute,
    private modalService: NgbModal,
    private cdr: ChangeDetectorRef
  ) {}

  selectTime(type) {
    this.selectedTime = type;

    let requestTime = '';

    switch (type) {
      case this.translate.instant('SMART_PLUG.TIME.LAST_24_HOURS'):
        requestTime = '24h';
        break;
      case this.translate.instant('SMART_PLUG.TIME.ONE_WEEK'):
        requestTime = '1w';
        break;
      case this.translate.instant('SMART_PLUG.TIME.ONE_MONTH'):
        requestTime = '1m';
        break;

      case this.translate.instant('SMART_PLUG.TIME.TWO_MONTHS'):
        requestTime = '2m';
        break;
      default:
        requestTime = '24h';
        break;
    }

    this.http
      .post(`${environment.apiUrl}/Smartplug`, {
        format: requestTime,
        fromDate: '',
        toDate: '',
      })
      .subscribe((res: any) => {
        this.smartPlug = res.outputs.find(
          (output) => output.baseDevice.id === +this.ar.snapshot.params.id
        );
        this.voltages = [];
        this.energies = [];
        this.labels = [];
        this.labelColors = [];

        this.smartPlug.infoModels.forEach((im) => {
          const date = new Date(im.time);

          switch (requestTime) {
            case '24h':
              this.labels.push(date.getHours().toString());
              break;
            case '1w':
              this.labels.push(this.setDay(date.getUTCDay()));
              break;
            case '1m':
              this.labels.push(
                date.toLocaleDateString('fa-IR')
              );
              break;
            case '2m':
              this.labels.push(
                date.toLocaleDateString('fa-IR')
              );
            //

            default:
              break;
          }

          this.setPeakTime()

          // this.labels.push(date.getHours() + ":" + date.getMinutes())
          this.voltages.push(im.voltage);
          this.energies.push(im.watt);
          this.labelColors.push('var(--primary-color)');
        });

        if (
          this.chartType === this.translate.instant('SMART_PLUG.LOOKUP.VOLTAGE')
        ) {
          this.chartData = this.voltages;
        } else {
          this.chartData = this.energies;
        }

        this.setChart();
      });
  }

  setDay(num: number) {
    let result = '';

    switch (num) {
      case 0:
        result = this.translate.instant('GENERAL.WEEK.SUNDAY');
        break;
      case 1:
        result = this.translate.instant('GENERAL.WEEK.MONDAY');
        break;
      case 2:
        result = this.translate.instant('GENERAL.WEEK.TUESDAY');
        break;
      case 3:
        result = this.translate.instant('GENERAL.WEEK.WEDNESDAY');
        break;
      case 4:
        result = this.translate.instant('GENERAL.WEEK.THURSDAY');
        break;
      case 5:
        result = this.translate.instant('GENERAL.WEEK.FRIDAY');
        break;
      case 6:
        result = this.translate.instant('GENERAL.WEEK.SATURDAY');
        break;
      default:
        break;
    }

    return result;
  }

  selectType(type) {
    this.chartType = type;

    if (type === this.translate.instant('SMART_PLUG.LOOKUP.VOLTAGE')) {
      this.chartData = this.voltages;
    } else {
      this.chartData = this.energies;
    }

    this.setChart();
  }

  getInformation() {
    this.http
      .post(`${environment.apiUrl}/Smartplug`, {
        format: '24h',
        fromDate: '',
        toDate: '',
      })
      .subscribe(
        (res: any) => {


          this.smartPlug = res.outputs.find(
            (output) => output.baseDevice.id === +this.ar.snapshot.params.id
          );



          this.smartPlug.infoModels.forEach((im) => {
            const date = new Date(im.time);
            this.labels.push(date.getHours().toString());
            this.voltages.push(im.voltage);
            this.energies.push(im.watt);
            this.labelColors.push('var(--primary-color)');
          });

          this.setPeakTime()

          this.chartData = this.energies;

          this.setChart();
        },
        (err) => {

        }
      );
  }

  setPeakTime(){
    let year = new Date().getFullYear()
    let firstSixMonth = false;
    var march21 = new Date(year+'-3-21')
    //
    var september22 = new Date(year+'-9-22')

    if(new Date() > march21 && new Date() < september22) firstSixMonth = true
    else firstSixMonth = false;

    if(firstSixMonth){
      if(+this.labels[0] > 17){
        this.x1Peak = this.labels[0]

        this.x2Peak = this.labels.find(l => l === '21')

        this.x1Peak2 = this.labels.find(l => l === '17')
        this.x2Peak2 = this.labels[this.labels.length - 1]
      }
      else{
        this.x1Peak = this.labels.find(l => l === '17')
        this.x2Peak = this.labels.find(l => l === '21')
      }
    }
    else {
      if(+this.labels[0] > 18){
        this.x1Peak = this.labels[0]

        this.x2Peak = this.labels.find(l => l === '22')

        this.x1Peak2 = this.labels.find(l => l === '18')
        this.x2Peak2 = this.labels[this.labels.length - 1]
      }
      else{
        this.x1Peak = this.labels.find(l => l === '18')
        this.x2Peak = this.labels.find(l => l === '22')
      }

    }

  }

  ngOnInit() {

    this.getInformation();
  }

  setChart() {
    this.chartOptions = {
      series: [
        {
          name: 'My-series',
          // data: [10, 41, 35, 51, 49, 62, 69, 91, 148, 10, 41, 35, 10, 41, 35, 51, 49, 62,
          //     69, 91, 148, 10, 41, 35],
          data: this.chartData,
        },
      ],
      chart: {
        id: 'chart',
        height: 300,
        type: 'area',

      },
      stroke: {
        colors: ['var(--primary-color)'],
      },
      yaxis: {
        labels: {
          style: {
            fontSize: '1rem',
            colors: [
              'var(--text-color)',
              'var(--text-color)',
              'var(--text-color)',
              'var(--text-color)',
              'var(--text-color)',
              'var(--text-color)',
              'var(--text-color)',
              'var(--text-color)',
              'var(--text-color)',
              'var(--text-color)',
              'var(--text-color)',
              'var(--text-color)',
              'var(--text-color)',
              'var(--text-color)',
            ],
          },
        },
      },
      xaxis: {
        categories: this.labels,
        labels: {
          style: {
            colors: this.labelColors,
            fontSize: '1.2rem',
            fontFamily : 'Vazir'
          },
        },
      },
      fill: {
        colors: ['var(--primary-color)'],
      },
      grid: {
        yaxis: {
          lines: {
            show: false,
          },
        },
      },
      dataLabels: {
        enabled: false,
      },
      annotations: {
        xaxis: [
          {
            x: this.x1Peak,
            x2: this.x2Peak,
            fillColor: 'var(--ui-red)',
            label: {
              text: this.translate.instant('SMART_PLUG.PEAK'),
              style: {
                background: 'var(--surface-d)',
                color: 'var(--text-color)',
                fontSize: '1.2rem',
                fontFamily: 'Vazir',
              },
            },
          },
          {
            x: this.x1Peak2,
            x2: this.x2Peak2,
            fillColor: 'var(--ui-red)',
            label: {
              text: this.translate.instant('SMART_PLUG.PEAK'),
              style: {
                background: 'var(--surface-d)',
                color: 'var(--text-color)',
                fontSize: '1.2rem',
                fontFamily: 'Vazir',
              },
            },
          },
        ],
      },
    };
  }

  open(content) {
    this.from = null
    this.to = null
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', centered: true, size: 'xl' });
  }

  // onDateSelection(date: NgbDate) {
  //   if (!this.fromDate && !this.toDate) {
  //     this.fromDate = date;
  //   } else if (this.fromDate && !this.toDate && date.after(this.fromDate)) {
  //     this.toDate = date;
  //   } else {
  //     this.toDate = null;
  //     this.fromDate = date;
  //   }
  // }

  selectStartDate(e) {
    this.from = new Date(e.timestamp);
    this.shamsiFrom = e.shamsi.split(' ')[0]

    if (this.to) {
      this.modalService.dismissAll()
      this.getRangeData()
    }
  }

  selectEndDate(e) {
    this.to = new Date(e.timestamp)
    this.shamsiTo = e.shamsi.split(' ')[0]

    if (this.from) {
      this.modalService.dismissAll()
      this.getRangeData()
    }
  }

  getRangeData() {
    this.http
    .post(`${environment.apiUrl}/Smartplug`, {
      format: '',
      fromDate: this.from.toISOString().split('T')[0].split('-').join('/'),
      toDate: this.to.toISOString().split('T')[0].split('-').join('/'),
    })
    .subscribe((res: any) => {


      this.smartPlug = res.outputs.find(
        (output) => output.baseDevice.id === +this.ar.snapshot.params.id
      );

      this.voltages = [];
      this.energies = [];
      this.labels = [];
      this.labelColors = [];

      this.smartPlug.infoModels.forEach((im) => {
        //

        //

        const d = new Date(im.time);
        this.labels.push(
          d.toLocaleDateString('fa-IR')
        );
        // this.labels.push(new Date(im.time).toISOString().split('T')[0].split('-').join('/'))
        this.voltages.push(im.voltage);
        this.energies.push(im.watt);
        this.labelColors.push('var(--primary-color)');
      });

      if (
        this.chartType === this.translate.instant('SMART_PLUG.LOOKUP.VOLTAGE')
      ) {
        this.chartData = this.voltages;
      } else {
        this.chartData = this.energies;
      }

      this.setChart();
    });
  }
}
