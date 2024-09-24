import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as CryptoJS from 'crypto-js';

@Injectable({
  providedIn: 'root',
})
export class TransactionService {
  private apiUrl = 'https://localhost:44387/api'; // Update with your API URL

  constructor(private http: HttpClient) {}

  async processTransaction(transactionData: any): Promise<any> {
    // Step 1: Get encryption key
    const keyResponse: any = await this.http
      .get(`${this.apiUrl}/Encryption/GenerateKey`)
      .toPromise();
    const key = keyResponse.key;
    // Step 2: Encrypt transaction data
    const encryptedData = this.encryptData(
      JSON.stringify(transactionData),
      key
    );

    // Step 3: Send encrypted data to the API
    const payload = {
      EncryptedData: encryptedData,
      Key: key, // In a real scenario, do not send the key back to the server
    };

    const response: any = await this.http
      .post(`${this.apiUrl}/Transaction/Process`, payload)
      .toPromise();
    console.log('Encrypted Data:- ', response.encryptedData);
    // Step 4: Decrypt response data
    const decryptedResponseData = this.decryptData(response.encryptedData, key);
    return JSON.parse(decryptedResponseData);
  }

  private encryptData(data: string, key: string): string {
    const keyBytes = CryptoJS.enc.Base64.parse(key);
    const iv = CryptoJS.lib.WordArray.random(16);

    const encrypted = CryptoJS.AES.encrypt(data, keyBytes, {
      iv: iv,
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7,
    });

    // Prepend IV to the encrypted data
    const encryptedData = iv
      .concat(encrypted.ciphertext)
      .toString(CryptoJS.enc.Base64);
    return encryptedData;
  }

  private decryptData(encryptedData: string, key: string): string {
    const keyBytes = CryptoJS.enc.Base64.parse(key);
    const encryptedDataBytes = CryptoJS.enc.Base64.parse(encryptedData);

    // Extract IV
    const iv = CryptoJS.lib.WordArray.create(
      encryptedDataBytes.words.slice(0, 4)
    );
    const ciphertext = CryptoJS.lib.WordArray.create(
      encryptedDataBytes.words.slice(4)
    );

    const decrypted = CryptoJS.AES.decrypt(
      { ciphertext: ciphertext },
      keyBytes,
      {
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7,
      }
    );

    return decrypted.toString(CryptoJS.enc.Utf8);
  }
}
