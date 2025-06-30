import { AbstractControl, FormControl, FormGroup, ValidatorFn } from '@angular/forms'

export class ValidationService {

    static childrenEqual: ValidatorFn = (formGroup: FormGroup) => {
        const [firstControlName, ...otherControlNames] = Object.keys(formGroup.controls || {})
        const isValid = otherControlNames.every(controlName => formGroup.get(controlName).value === formGroup.get(firstControlName).value)
        return isValid ? null : { invalid: true }
    }

    static validDatePeriod: ValidatorFn = (formGroup: FormGroup) => {
        const [firstControlName, ...otherControlNames] = Object.keys(formGroup.controls || {})
        const isValid = otherControlNames.every(controlName => formGroup.get(controlName).value >= formGroup.get(firstControlName).value)
        return isValid ? null : { invalid: true }
    }

    static beginsOrEndsWithSpace(control: AbstractControl): { [key: string]: any } {
        const pattern = /(^\s+)|(\s+$)/
        return pattern.test(control.value) ? { beginsOrEndsWithSpace: true } : null
    }

    static containsSpace(control: AbstractControl): { [key: string]: any } {
        return control.value && (control.value as string).indexOf(' ') !== -1 ? { containsSpace: true } : null
    }

    static doesNotContainUpperCase(control: AbstractControl): { [key: string]: any } {
        const pattern = /[A-Z]/
        return pattern.test(control.value) ? null : { doesNotContainUpperCase: true }
    }

    static doesNotContainLowerCase(control: AbstractControl): { [key: string]: any } {
        const pattern = /[a-z]/
        return pattern.test(control.value) ? null : { doesNotContainLowerCase: true }
    }

    static doesNotContainDigits(control: AbstractControl): { [key: string]: any } {
        const pattern = /[0-9]/
        return pattern.test(control.value) ? null : { doesNotContainDigits: true }
    }

    static doesNotContainSymbol(control: AbstractControl): { [key: string]: any } {
        const pattern = /[!@#$%^&*()]/
        return pattern.test(control.value) ? null : { doesNotContainSymbol: true }
    }

    static containsIllegalCharacters(control: AbstractControl): { [key: string]: any } {
        const pattern = /^[a-zA-Z\d-_]+$/
        return pattern.test(control.value) ? null : { containsIllegalCharacters: true }
    }

    static isGreaterThanZero(control: AbstractControl): { [key: string]: any } {
        return control.value == 0 ? { isGreaterThanZero: false } : null
    }

    static shouldBeCapitalLetterOrSpace(control: AbstractControl): { [key: string]: any } {
        const pattern = /^[A-Z]+([ A-Z]+)?$/
        return pattern.test(control.value) ? null : { shouldBeCapitalLetterOrSpace: true }
    }

    static shouldBeOnlyNumbers(control: AbstractControl): { [key: string]: any } {
        const pattern = /[0-9]/
        return pattern.test(control.value) ? null : { shouldBeOnlyNumbers: true }
    }

    static shouldBeFiveCapitalLetters(control: AbstractControl): { [key: string]: any } {
        const pattern = /[A-Z]{5}/
        return pattern.test(control.value) ? null : { shouldBeFiveCapitalLetters: true }
    }

    static shouldBeEmptyPlusOrMinus(control: AbstractControl): { [key: string]: any } {
        if (control.value == '') {
            return null
        } else {
            const pattern = /[+-]/
            return pattern.test(control.value) ? null : { shouldBeEmptyPlusOrMinus: true }
        }
    }

    static isTime(control: AbstractControl): { [key: string]: any } {
        if (control.value) {
            const pattern = /\b([01][0-9]|2[0-3]):([0-5][0-9])\b/g
            return pattern.test(control.value) ? null : { isTime: false }
        }
    }

    static ageIsLessThanOneHundredYears(control: AbstractControl): any {
        if (control.value) {
            const today = new Date().getFullYear()
            const given = new Date(control.value).getFullYear()
            return today - given <= 100 ? null : { ageIsLessThanOneHundredYears: true }
        }
        return null
    }

    static RequireAutocomplete(control: AbstractControl): any {
        const selection: any = control.value
        if (typeof selection === 'string') {
            return { incorrect: true }
        }
        return null
    }

}

export class ConfirmValidParentMatcher {

    isErrorState(control: FormControl | null): boolean {
        return control.parent.invalid && control.touched
    }

}
