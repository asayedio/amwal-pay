export class BcdConverter {
  static stringToBcd(input: string): string {
    if (input.length % 2 !== 0) {
      input = '0' + input;
    }
    let bcd = '';
    for (let i = 0; i < input.length; i += 2) {
      bcd += String.fromCharCode(parseInt(input.substr(i, 2), 16));
    }
    return bcd;
  }

  static bcdToString(bcd: string): string {
    let result = '';
    for (let i = 0; i < bcd.length; i++) {
      const byte = bcd.charCodeAt(i);
      result += ('0' + (byte >> 4).toString(16)).slice(-1);
      result += ('0' + (byte & 0x0f).toString(16)).slice(-1);
    }
    return result.replace(/^0+/, ''); // Remove leading zeros
  }
}
