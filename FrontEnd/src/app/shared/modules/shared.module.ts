import { CommonModule } from '@angular/common'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { NgModule } from '@angular/core'
import { NgxCurrencyDirective, NgxCurrencyInputMode, provideEnvironmentNgxCurrency } from 'ngx-currency'
import { PrimeNgModule } from './primeng.module'
import { RouterModule } from '@angular/router'
import { ZXingScannerModule } from '@zxing/ngx-scanner'
// Custom
import { AbsPipe } from '../pipes/abs.pipe'
import { CatPageComponent } from '../components/cat-page/cat-page.component'
import { CriteriaDateRangeDialogComponent } from '../components/criteria-date-range-dialog/criteria-date-range-dialog.component'
import { CriteriaFieldsetCheckboxesComponent } from '../components/criteria-fieldset-checkboxes/criteria-fieldset-checkboxes.component'
import { CriteriaFieldsetRadiosComponent } from '../components/criteria-fieldset-radios/criteria-fieldset-radios.component'
import { CriteriaFieldsetWeekdaysComponent } from '../components/criteria-fieldset-weekdays/criteria-fieldset-weekdays.component'
import { DatePickerPillComponent } from '../components/date-picker-pill/date-picker-pill.component'
import { DateRangePickerComponent } from '../components/date-range-picker/date-range-picker.component'
import { DeleteRangeDialogComponent } from '../components/delete-range-dialog/delete-range-dialog.component'
import { EmojiDirective } from '../directives/emoji.directive'
import { EmptyPageComponent } from '../components/empty-page/empty-page.component'
import { HomeButtonAndTitleComponent } from '../components/home-button-and-title/home-button-and-title.component'
import { InputMaxLengthDirective } from '../directives/input-maxLength.directive'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { LanguageMenuComponent } from '../components/language-menu/language-menu.component'
import { LoadingSpinnerComponent } from '../components/loading-spinner/loading-spinner.component'
import { LogoComponent } from '../components/logo/logo.component'
import { MainFooterComponent } from '../components/home/main-footer.component'
import { MaterialModule } from './material.module'
import { MetadataPanelComponent } from '../components/metadata-panel/metadata-panel.component'
import { ModalDialogComponent } from '../components/modal-dialog/modal-dialog.component'
import { MonthSelectorComponent } from '../components/month-selector/month-selector.component'
import { PadNumberPipe } from '../pipes/pad-number.pipe'
import { PrettyPrintPipe } from '../pipes/json-pretty.pipe'
import { ReplaceZeroPipe } from '../pipes/replace-zero.pipe'
import { SafeStylePipe } from '../pipes/safe-style.pipe'
import { TableTotalFilteredRecordsComponent } from '../components/table-total-filtered-records/table-total-filtered-records.component'
import { ThemeSelectorComponent } from '../components/theme-selector/theme-selector.component'
import { TrimStringPipe } from './../pipes/string-trim.pipe'
import { YearSelectorComponent } from '../components/year-selector/year-selector.component'

@NgModule({
    declarations: [
        AbsPipe,
        CatPageComponent,
        CriteriaDateRangeDialogComponent,
        CriteriaFieldsetCheckboxesComponent,
        CriteriaFieldsetRadiosComponent,
        CriteriaFieldsetWeekdaysComponent,
        DatePickerPillComponent,
        DateRangePickerComponent,
        DeleteRangeDialogComponent,
        EmojiDirective,
        EmptyPageComponent,
        HomeButtonAndTitleComponent,
        InputMaxLengthDirective,
        InputTabStopDirective,
        LanguageMenuComponent,
        LoadingSpinnerComponent,
        LogoComponent,
        MainFooterComponent,
        MetadataPanelComponent,
        ModalDialogComponent,
        MonthSelectorComponent,
        PadNumberPipe,
        PrettyPrintPipe,
        ReplaceZeroPipe,
        SafeStylePipe,
        TableTotalFilteredRecordsComponent,
        ThemeSelectorComponent,
        TrimStringPipe,
        YearSelectorComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        MaterialModule,
        NgxCurrencyDirective,
        PrimeNgModule,
        ReactiveFormsModule,
        RouterModule,
        ZXingScannerModule,
    ],
    exports: [
        AbsPipe,
        CatPageComponent,
        CommonModule,
        CriteriaDateRangeDialogComponent,
        CriteriaFieldsetCheckboxesComponent,
        CriteriaFieldsetRadiosComponent,
        CriteriaFieldsetWeekdaysComponent,
        DatePickerPillComponent,
        DateRangePickerComponent,
        DeleteRangeDialogComponent,
        EmojiDirective,
        EmptyPageComponent,
        FormsModule,
        HomeButtonAndTitleComponent,
        InputMaxLengthDirective,
        InputTabStopDirective,
        LanguageMenuComponent,
        LoadingSpinnerComponent,
        LogoComponent,
        MainFooterComponent,
        MaterialModule,
        MetadataPanelComponent,
        MonthSelectorComponent,
        NgxCurrencyDirective,
        PadNumberPipe,
        PrettyPrintPipe,
        PrimeNgModule,
        ReactiveFormsModule,
        ReplaceZeroPipe,
        RouterModule,
        RouterModule,
        TableTotalFilteredRecordsComponent,
        ThemeSelectorComponent,
        TrimStringPipe,
        YearSelectorComponent,
        ZXingScannerModule
    ], providers: [
        provideEnvironmentNgxCurrency({
            align: 'right',
            allowNegative: true,
            allowZero: true,
            decimal: '.',
            inputMode: NgxCurrencyInputMode.Natural,
            max: null,
            min: null,
            nullable: true,
            precision: 2,
            prefix: '€ ',
            suffix: '',
            thousands: ','
        })
    ]
})

export class SharedModule { }
