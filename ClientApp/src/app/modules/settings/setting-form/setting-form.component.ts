import { HttpClient } from "@angular/common/http";
import { Component, OnInit, ViewChild, ViewEncapsulation } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { NotifierService } from "angular-notifier";
import { Subscription } from "rxjs";
import { tap } from "rxjs/operators";
import Keyboard from 'simple-keyboard'
import layout from 'simple-keyboard-layouts/build/layouts/farsi'
import { environment } from "src/environments/environment";
import { HomeService } from "../../home/_services/home.service";
import { ModuleService } from "../../home/_services/module.service";
@Component({
    selector: 'sh-setting-form',
    templateUrl: './setting-form.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: [
        "../../../../../node_modules/simple-keyboard/build/css/index.css",
        "./setting-form.component.scss"
    ]
})
export class SettingFormComponent implements OnInit {
    @ViewChild('notif', { static: true }) notifTemp;

    keyboard: Keyboard;
    inputName = "name";
    inputs = {
        name: "",
        serial: "",
        pin: ""
    };
    deviceType: string;
    module: string;
    relys: any;

    device: any;

    config = {
        hasName: false,
        hasSerial: false,
        hasType: false,
        hasModule: false,
        hasPin: false
    }
    detecting: boolean = false;

    private _subscriptions: Subscription[] = []


    title: string;
    btnValue: string;
    lang: string = 'persian'
    constructor(
        private active: ActivatedRoute,
        private http: HttpClient,
        private homeService: HomeService,
        private moduleService: ModuleService,
        private router: Router,
        private notifierService: NotifierService,
        public translate: TranslateService) { }

    selectTime(time) {

    }

    ngOnInit() {
        this.setConfig()

        this.inputs = {
            name: '',
            serial: '',
            pin: ''
        }

        this.moduleService.fetchModules()
        this.moduleService._modules$.subscribe(m => {
            this.relys = m

            if (this.active.snapshot.params.type && this.active.snapshot.params.type === 'LED') {
                for (let r in this.homeService._home$.value.rooms) {
                    const dev = this.homeService._home$.value.rooms[r].devices.leDs.find(l => l.id === +this.active.snapshot.params.id)
                    //
                    if (dev) {
                        //
                        this.device = dev
                        break;
                    }


                }
                this.module = this.relys.find(m => m.id === this.device.moduleParentId).name


                this.title = this.translate.instant('SETTINGS.FORM.EDIT_LED')
                this.btnValue = this.translate.instant('SETTINGS.FORM.EDIT')

                this.inputs.name = this.device.name
                this.inputs.pin = this.device.registerid.toString()
            }
            else if (this.active.snapshot.params.type && this.active.snapshot.params.type === 'Curtain') {
                for (let r in this.homeService._home$.value.rooms) {
                    const dev = this.homeService._home$.value.rooms[r].devices.curtains.find(c => c.id === +this.active.snapshot.params.id)
                    //
                    if (dev) {
                        //
                        this.device = dev
                        break;
                    }


                }

                this.title = this.translate.instant('SETTINGS.FORM.EDIT_CURTAIN')
                this.btnValue = this.translate.instant('SETTINGS.FORM.EDIT')

                this.module = this.relys.find(m => m.id === this.device.moduleParentId).name
                this.inputs.name = this.device.name
                this.inputs.pin = this.device.registerid.toString()
            }
            else if (this.active.snapshot.params.type && this.active.snapshot.params.type === 'Windoor') {
                for (let r in this.homeService._home$.value.rooms) {
                    const dev = this.homeService._home$.value.rooms[r].devices.windoors.find(w => w.id === +this.active.snapshot.params.id)
                    //
                    if (dev) {
                        //
                        this.device = dev
                        break;
                    }


                }

                this.title = 'Edit Windows And Doors'
                this.btnValue = this.translate.instant('SETTINGS.FORM.EDIT')

                this.module = this.relys.find(m => m.id === this.device.moduleParentId).name
                this.inputs.name = this.device.name
                // this.inputs.pin = this.device.registerid.toString()
            }
            else this.module = this.relys[0]
        })


        if (this.active.snapshot.url.length > 1 && this.active.snapshot.url[1].path === 'module') {
          this.btnValue = this.translate.instant('SETTINGS.SETTINGS_CARD.AUTO_DETECT');
          this.detecting = true;

          this.http.get(`${environment.apiUrl}/Module/autodetect`).subscribe(
            (res: any) => {
              this.detecting = false;
              this.btnValue = this.translate.instant('SETTINGS.FORM.ADD');
              if (res && res.serialNumber) this.inputs.serial = res.serialNumber;
              else this.notifierService.notify('error', this.translate.instant('SETTINGS.SETTINGS_CARD.AUTO_DETECT_FAILED'))
            },
            err => {
              this.detecting = false;
              this.btnValue = this.translate.instant('SETTINGS.FORM.ADD');
              this.notifierService.notify('error', this.translate.instant('SETTINGS.SETTINGS_CARD.AUTO_DETECT_FAILED'))
            }
          );
        }
    }

    setConfig() {

        if (this.active.snapshot.params.id) {
            if (this.active.snapshot.params.type === 'Windoor') {
                this.config = {
                    hasName: true,
                    hasModule: false,
                    hasPin: false,
                    hasType: false,
                    hasSerial: false
                }
            }
            else if (this.active.snapshot.params.type === 'Module') {
                this.config = {
                    hasName: false,
                    hasModule: false,
                    hasPin: false,
                    hasSerial: true,
                    hasType: false
                }

                this.moduleService._modules$.subscribe(m =>{
                    let modules = m
                    this.inputs.serial = modules.find(module => module.id === +this.active.snapshot.params.id).serialNumber
                })

                this.title = this.translate.instant('SETTINGS.FORM.EDIT_MODULE')
                this.btnValue = this.translate.instant('SETTINGS.FORM.EDIT')
            }
            else {
                this.config = {
                    hasName: true,
                    hasModule: true,
                    hasPin: true,
                    hasType: false,
                    hasSerial: false
                }
            }
            // document.querySelector('.setting-form').classList.add('edit-form')
            // document.querySelector('.setting-form').classList.remove('setting-form')
            // this.title = this.translate.instant('SETTINGS.FORM.EDIT_TITLE')


        }
        else if (this.active.snapshot.url.length > 1 && this.active.snapshot.url[1].path === 'module') {
            this.config = {
                hasName: false,
                hasModule: false,
                hasPin: false,
                hasType: false,
                hasSerial: true
            }
            // this.title = this.translate.instant('SETTINGS.FORM.ADD_TITLE')
            this.title = this.translate.instant('SETTINGS.FORM.ADD_NEW_MODULE')
            this.btnValue = this.translate.instant('SETTINGS.FORM.ADD')
        }
        else {
            // this.title = this.translate.instant('SETTINGS.FORM.ADD_TITLE')
            this.config = {
                hasName: true,
                hasModule: true,
                hasPin: true,
                hasType: true,
                hasSerial: true
            }

            this.title = this.translate.instant('SETTINGS.FORM.ADD_NEW_DEVICE')
            this.btnValue = this.translate.instant('SETTINGS.FORM.ADD')

        }
    }

    ngAfterViewInit() {
        if (this.config.hasName) {
            this.keyboard = new Keyboard({
                // debug: true,
                inputName: this.inputName,
                onChange: (input) => this.onChange(input),
                onKeyPress: (button) => this.onKeyPress(button),
                ...layout,
                // rtl: true,
                theme: 'hg-theme-default myTheme1',
                preventMouseDownDefault: true // If you want to keep focus on input

            });
            this.keyboard.replaceInput(this.inputs);
            document.getElementById('s').style.width = '100%'
        }
        else {
            this.keyboard = new Keyboard({
                // debug: true,
                inputName: this.inputName,
                onChange: (input) => this.onChange(input),
                onKeyPress: (button) => this.onKeyPress(button),
                layout: {
                    default: ["1 2 3", "4 5 6", "7 8 9", "{shift} 0 _", "{bksp}"],
                    shift: ["! / #", "$ % ^", "& * (", "{shift} ) +", "{bksp}"]
                },
                // rtl: false,
                theme: 'hg-theme-default myTheme1',
                preventMouseDownDefault: true // If you want to keep focus on input

            });
            this.keyboard.replaceInput(this.inputs);
            document.getElementById('s').style.width = '50%'
            document.getElementById('s').style.marginLeft = 'auto'
            document.getElementById('s').style.marginRight = 'auto'
        }
    }

    onInputFocus = (event: any) => {
        this.inputName = event.target.id;
        //
        if (this.inputName === 'serial' || this.inputName === 'pin') {
            this.keyboard.setOptions({
                inputName: event.target.id,
                layout: {
                    default: ["1 2 3", "4 5 6", "7 8 9", "0 - {bksp}", "{space}"],
                    // shift: ["! / #", "$ % ^", "& * (", "{shift} ) +", "{bksp}"]
                },
                // rtl: false
            });

            document.getElementById('s').style.width = '50%'
            document.getElementById('s').style.marginLeft = 'auto'
            document.getElementById('s').style.marginRight = 'auto'
        }
        else {
            if (this.lang === 'persian') {
                this.keyboard.setOptions({
                    inputName: event.target.id,
                    ...layout,
                    // rtl: true
                });

            }
            else {
                this.keyboard.setOptions({
                    layout: {
                        default: [
                            "` 1 2 3 4 5 6 7 8 9 0 - = {bksp}",
                            "{tab} q w e r t y u i o p [ ] \\",
                            "{lock} a s d f g h j k l ; ' {enter}",
                            "{shift} z x c v b n m , . / {shift}",
                            ".com @ {space}",
                        ],
                        shift: [
                            "~ ! @ # $ % ^ & * ( ) _ + {bksp}",
                            "{tab} Q W E R T Y U I O P { } |",
                            '{lock} A S D F G H J K L : " {enter}',
                            "{shift} Z X C V B N M < > ? {shift}",
                            ".com @ {space}",
                        ],
                    },
                    // rtl: false
                })
            }

            document.getElementById('s').style.width = '100%'
        }

    };

    // setInputCaretPosition = (elem: any, pos: number) => {
    //     if (elem.setSelectionRange) {
    //         elem.focus();
    //         elem.setSelectionRange(pos, pos);
    //     }
    // };

    onInputChange = (event: any) => {
        this.keyboard.setInput(event.target.value, event.target.id);

    };

    onChange = (input: string) => {
        this.inputs[this.inputName] = input;



        // let caretPosition = this.keyboard.caretPosition;

        // if (caretPosition !== null)
        //     this.setInputCaretPosition(
        //         document.querySelector(`#${this.inputName}`),
        //         caretPosition
        //     );

    };

    onKeyPress = (button: string) => {
        if (button === "{shift}" || button === "{lock}") this.handleShift();
    };

    changeLang(event) {

        this.lang = event.target.value;

        if (this.lang === 'english' && this.inputName !== 'serial') {
            this.keyboard.setOptions({
                layout: {
                    default: [
                        "` 1 2 3 4 5 6 7 8 9 0 - = {bksp}",
                        "{tab} q w e r t y u i o p [ ] \\",
                        "{lock} a s d f g h j k l ; ' {enter}",
                        "{shift} z x c v b n m , . / {shift}",
                        ".com @ {space}",
                    ],
                    shift: [
                        "~ ! @ # $ % ^ & * ( ) _ + {bksp}",
                        "{tab} Q W E R T Y U I O P { } |",
                        '{lock} A S D F G H J K L : " {enter}',
                        "{shift} Z X C V B N M < > ? {shift}",
                        ".com @ {space}",
                    ],
                },
                // rtl: false
            })

        }
        else if (this.lang === 'persian' && this.inputName !== 'serial') {
            this.keyboard.setOptions({
                ...layout,
                // rtl: true

            })

        }

    }

    handleShift = () => {
        let currentLayout = this.keyboard.options.layoutName;
        let shiftToggle = currentLayout === "default" ? "shift" : "default";

        this.keyboard.setOptions({
            layoutName: shiftToggle
        });
    };



    handleEvent() {
      if (this.detecting) return;

        if (this.active.snapshot.url.length > 1 && this.active.snapshot.url[1].path === 'module') {
            if (this.inputs.serial.trim().length > 0) {


                const request = this.http.put(`${environment.apiUrl}/Module`, {
                    serialNumber: this.inputs.serial,
                }).subscribe(
                    res => {
                        this.notifierService.notify('success', this.translate.instant('NOTIFICATION.ADDED_SUCCESSFULLY'))
                        setTimeout(() => {
                            setTimeout(() => { this.router.navigateByUrl('/settings') }, 1000)
                        }, 3000)

                    },
                    err => {

                        if (err.error === 'duplicate serial detected') {
                            this.notifierService.notify('error', this.translate.instant('NOTIFICATION.DUPLICATE_MODULE_ERROR'))
                        }

                    }
                )

                this._subscriptions.push(request)
            }
        }
        else if (this.active.snapshot.params.id) {
            if (this.active.snapshot.params.type === 'Windoor') {
                if (this.inputs.name.trim().length > 0) {



                    let modulepI = this.relys.find(r => r.serialNumber === this.module)


                    let obj = {
                        registerId: this.device.registerId,
                        id: this.device.id,
                        roomId: this.homeService._selectedRoom$.value.roomId,
                        moduleParentId: modulepI.id,
                        name: this.inputs.name,
                        deviceType: this.device.deviceType,
                        serialNumber: this.device.serialNumber
                    }





                    this.http.post(`${environment.apiUrl}/Device`, obj).subscribe(
                        res => {
                            this.notifierService.notify('success', this.translate.instant('NOTIFICATION.CHANGED_SUCCESSFULLY'))
                            setTimeout(() => { this.router.navigateByUrl('/settings') }, 1000)
                            this.homeService.fetchHome()
                            this.homeService._selectedRoom$.next(null)

                        },
                        err => {
                            this.notifierService.notify('error', this.translate.instant('NOTIFICATION.DUPLICATE_DEVICE_ERROR'))

                        }
                    )
                }
            }
            else if (this.active.snapshot.params.type === 'Module') {
                if (this.inputs.serial.trim().length > 0) {
                    this.http.post(`${environment.apiUrl}/Module`, {
                        serialNumber: this.inputs.serial,
                        id: +this.active.snapshot.params.id
                    }).subscribe(
                        res => {
                            const newModules = this.moduleService._modules$.value

                            this.notifierService.notify('success', this.translate.instant('NOTIFICATION.CHANGED_SUCCESSFULLY'))
                            setTimeout(() => { this.router.navigateByUrl('/settings') }, 1000)
                        },
                        err => {
                            this.notifierService.notify('error', this.translate.instant('NOTIFICATION.DUPLICATE_MODULE_ERROR'))

                        }
                    )
                }
            }
            else {
                if (this.inputs.name.trim().length > 0 && this.inputs.pin.trim().length > 0) {



                    let modulepI = this.relys.find(r => r.serialNumber === this.module)


                    let obj = {
                        registerId: +this.inputs.pin.substring(1, this.inputs.pin.length - 1),
                        id: this.device.id,
                        roomId: this.homeService._selectedRoom$.value.roomId,
                        moduleParentId: modulepI.id,
                        name: this.inputs.name,
                        deviceType: this.device.deviceType,
                        serialNumber: this.device.serialNumber
                    }





                    this.http.post(`${environment.apiUrl}/Device`, obj).subscribe(
                        res => {
                            this.notifierService.notify('success', this.translate.instant('NOTIFICATION.CHANGED_SUCCESSFULLY'))
                            setTimeout(() => { this.router.navigateByUrl('/settings') }, 1000)
                            this.homeService.fetchHome()
                            this.homeService._selectedRoom$.next(null)

                        },
                        err => {
                            this.notifierService.notify('error', this.translate.instant('NOTIFICATION.DUPLICATE_DEVICE_ERROR'))

                        }
                    )
                }
            }


        }
        else if (this.active.snapshot.routeConfig.path.trim() === 'add') {


            if (this.inputs.name.trim().length > 0 && this.inputs.serial.trim().length > 0 && this.inputs.pin.trim().length > 0) {


                let obj = {
                    moduleParentId: this.relys.find(r => r.serialNumber === this.module).id,
                    registerId: +this.inputs.pin.substring(1, this.inputs.pin.length - 1),
                    roomId: this.homeService._selectedRoom$.value.roomId,
                    name: this.inputs.name,
                    serialNumber: this.inputs.serial
                }




                this.http.put(`${environment.apiUrl}/Device`, obj).subscribe(
                    res => {
                        this.notifierService.notify('success', this.translate.instant('NOTIFICATION.ADDED_SUCCESSFULLY'))
                        this.homeService._selectedRoom$.next(null)
                        setTimeout(() => { this.router.navigateByUrl('/settings') }, 1000)

                    },
                    err => {
                        this.notifierService.notify('error', this.translate.instant('NOTIFICATION.DUPLICATE_DEVICE_ERROR'))


                    }
                )
            }

        }


    }

    // onAdd() {
    //     if (this.inputs.name.trim().length > 0 && this.inputs.serial.trim().length > 0) {


    //         const req = this.http.put(`${environment.apiUrl}/Module`, {
    //             roomId: +this.homeService._selectedRoom$.value.roomId,
    //             name: this.inputs.name,
    //             serialNumber: this.inputs.serial.trim()
    //         }).subscribe(
    //             res => {
    //                 this.notifierService.notify('success', 'با موفقیت اضافه شد . درحال بازگشت به تنظیمات')

    //                 setTimeout(() => {
    //                     this.router.navigateByUrl('/settings')
    //                 }, 3000)
    //             },
    //             err => {

    //                 if (err.error === 'duplicate serial detected') {
    //                     this.notifierService.notify('error', 'یک ماژول دیگر با این شماره سریال موجود است')
    //                 }
    //                 else {
    //                     this.notifierService.notify('error', 'اشکال در اضافه کردن ماژول')
    //                 }

    //             }
    //         )

    //         this._subscriptions.push(req)
    //     }
    // }



    goToDetails() {
        this.router.navigateByUrl('/details')
    }
}
